using Persona.Entidades;
using Persona.Entidades.Dtos;
using Persona.Framework.Excepciones;
using Persona.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persona.Dominio
{
    public class PersonaDominio : IPersonaDominio
    {
        readonly IRepositorio<Entidades.Persona> PersonaRepositorio;
        readonly IRepositorio<PersonaContacto> PersonaContactoRepositorio;

        public PersonaDominio(IRepositorio<Entidades.Persona> _personaRepositorio, IRepositorio<Entidades.PersonaContacto> _personaContactoRepositorio)
        {
            PersonaRepositorio = _personaRepositorio;
            PersonaContactoRepositorio = _personaContactoRepositorio;
        }

        public async Task<List<dtoPersona>> ObtenerListadoPersonasAsync()
        {
            List<Entidades.Persona> personas = await PersonaRepositorio.ObtenerListadoAsync(p => p.Pais, p => p.TipoDocumento, p => p.PersonaContacto, p => p.Sexo);

            var dtoPersonas = (from persona in personas
                               select new dtoPersona
                               {
                                   Apellido = persona.Apellido,
                                   Nombre = persona.Nombre,
                                   Id = persona.Id,
                                   NumeroDocumento = persona.NumeroDocumento,
                                   FechaNacimiento = persona.FechaNacimiento,
                                   IdSexo = persona.IdSexo,
                                   Sexo = persona.Sexo.Descripcion,
                                   TipoDocumento = persona.TipoDocumento.Descripcion,
                                   Pais = persona.Pais.Descripcion,
                                   IdTipoDocumento = persona.IdTipoDocumento,
                                   IdPais = persona.IdPais,
                                   PersonaContacto = (from contactos in persona.PersonaContacto
                                                select new dtoContacto
                                                {
                                                    idContacto = contactos.Id,
                                                    idPersona = contactos.IdPersona,
                                                    Valor = contactos.Valor
                                                }).ToList(),
                               }).ToList();

            return dtoPersonas;
        }

        public async Task<dtoPersona> ObtenerPersonaAsync(int id)
        {
            Entidades.Persona persona = await ObtenerPersonaPrivadoAsync(id);
            if (persona == null)
            {
                return null;
            }
            return MapearEntidadADto(persona);
        }
    
        public async Task<dtoPersona> GuardarPersonaAsync(Entidades.Persona personaAGuardar)
        {
            try
            {
                Entidades.Persona persona = new Entidades.Persona();
                await ValidarDatosObligatoriosPersona(personaAGuardar);

                if (personaAGuardar.Id == 0)
                {
                    persona = await PersonaRepositorio.InsertarAsync(persona);
                }
                else
                {
                    persona = await ObtenerPersonaPrivadoAsync(personaAGuardar.Id);

                    persona.Apellido = personaAGuardar.Apellido;
                    persona.FechaNacimiento = personaAGuardar.FechaNacimiento;
                    persona.IdPais = personaAGuardar.IdPais;
                    persona.IdSexo = personaAGuardar.IdSexo;
                    persona.IdTipoDocumento = personaAGuardar.IdTipoDocumento;
                    persona.Nombre = personaAGuardar.Nombre;
                    persona.NumeroDocumento = personaAGuardar.NumeroDocumento;

                    foreach (PersonaContacto contacto in persona.PersonaContacto)
                    {
                        if (contacto.Id == 0)
                        {
                            await PersonaContactoRepositorio.InsertarAsync(new PersonaContacto { IdPersona = persona.Id, Valor = contacto.Valor });
                        }
                        else
                        {
                            PersonaContacto personaContacto = await PersonaContactoRepositorio.ObtenerUnoAsync(c => c.Id == contacto.Id);
                            if (personaContacto != null)
                            {
                                personaContacto.Valor = contacto.Valor;
                                await PersonaContactoRepositorio.ActualizarAsync(personaContacto);
                            }
                        }
                    }

                    await PersonaRepositorio.ActualizarAsync(persona);
                }

                return await ObtenerPersonaAsync(persona.Id);
            }
            catch (AccesoADatosException ex)
            {
                if (ex.Data.Count > 0)
                {
                    DatosInvalidosException datosInvalidos = new DatosInvalidosException();
                    throw datosInvalidos;
                }
            }
            finally
            {
                throw new Exception();
            }
        }

        public async Task EliminarPersonaAsync(int id)
        {
            Entidades.Persona persona = await ObtenerPersonaPrivadoAsync(id);
            foreach (var item in persona.PersonaContacto)
            {
                await PersonaContactoRepositorio.EliminarAsync(item);
            }
            await PersonaRepositorio.EliminarAsync(persona);
        }

        public async Task<dtoEstadisticas> ObtenerEstadisticasAsync()
        {
            int cantidadMujeres = 0;
            int cantidadHombres = 0;
            int cantidadArgentinos = 0;

            List<Entidades.Persona> personas = await PersonaRepositorio.ObtenerListadoAsync();

            int totalPersonas = personas.Count();

            for (int i = 0; i < totalPersonas; i++)
            {
                if (personas[i].IdSexo == (int)Entidades.Enumeraciones.SexoEnum.Masculino)
                {
                    cantidadHombres += 1;
                }
                else
                {
                    cantidadMujeres += 1;
                }

                if (personas[i].IdPais == (int)Entidades.Enumeraciones.PaisEnum.Argentina)
                {
                    cantidadArgentinos += 1;
                }
            }

            return new dtoEstadisticas()
            {
                CantidadHombres = cantidadHombres,
                CantidadMujeres = cantidadMujeres,
                PorcentajeArgentinos = cantidadArgentinos * 100 / totalPersonas == 0 ? 1 : totalPersonas
            };
        }

        private async Task<bool> ValidarDatosObligatoriosPersona(Entidades.Persona persona)
        {
            Framework.Validaciones.Validaciones.Validar<Entidades.Persona>(persona);

            DatosInvalidosException datosInvalidos = new DatosInvalidosException();

            if (await PersonaRepositorio.ObtenerUnoAsync(p => p.IdPais == persona.IdPais && p.IdTipoDocumento == persona.IdTipoDocumento && p.IdSexo == persona.IdSexo && p.NumeroDocumento == persona.NumeroDocumento && p.Id != persona.Id) != null)
            {
                datosInvalidos.Data.Add("Persona Existente", "La persona ya se encuentra registrada.");
            }

            if (persona.PersonaContacto.Count == 0)
            {
                datosInvalidos.Data.Add("Persona sin contactos", "Debe indicar al menos un tipo de contacto");
            }

            if (persona.FechaNacimiento.AddYears(18) > DateTime.Now)
            {
                datosInvalidos.Data.Add("Persona menor de edad", "La persona debe ser mayor a 18 años.");
            }

            if (datosInvalidos.Data.Count == 0)
            {
                return true;
            }
            else
            {
                throw datosInvalidos;
            }
        }

        private dtoPersona MapearEntidadADto(Entidades.Persona persona)
        {
            return new dtoPersona
            {
                Apellido = persona.Apellido,
                Nombre = persona.Nombre,
                Id = persona.Id,
                NumeroDocumento = persona.NumeroDocumento,
                FechaNacimiento = persona.FechaNacimiento,
                IdSexo = persona.IdSexo,
                Sexo = persona.Sexo.Descripcion,
                TipoDocumento = persona.TipoDocumento.Descripcion,
                Pais = persona.Pais.Descripcion,
                IdTipoDocumento = persona.IdTipoDocumento,
                IdPais = persona.IdPais,
                PersonaContacto = (from contactos in persona.PersonaContacto
                             select new dtoContacto
                             {
                                 idContacto = contactos.Id,
                                 idPersona = contactos.IdPersona,
                                 Valor = contactos.Valor
                             }).ToList(),
            };
        }

        private async Task<Entidades.Persona> ObtenerPersonaPrivadoAsync(int id)
        {
            return await PersonaRepositorio.ObtenerUnoAsync(p => p.Id == id, p => p.Pais, p => p.TipoDocumento, p => p.PersonaContacto, p => p.Sexo);           
        }
    }
}
