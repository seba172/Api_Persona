using Persona.Entidades;
using Persona.Entidades.Dtos;
using Persona.Framework.Excepciones;
using Persona.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Persona.Dominio
{
    public class PersonaDominio : IPersonaDominio
    {
        readonly IRepositorio<Entidades.Persona> PersonaRepositorio;
        readonly IRepositorio<PersonaContacto> PersonaContactoRepositorio;
        readonly IRepositorio<PersonaRelacion> PersonaRelacionRepositorio;
        readonly IRepositorio<TipoRelacion> TipoRelacionRepositorio;

        public PersonaDominio(IRepositorio<Entidades.Persona> _personaRepositorio, IRepositorio<Entidades.PersonaContacto> _personaContactoRepositorio, IRepositorio<PersonaRelacion> _personaRelacionRepositorio, IRepositorio<TipoRelacion> _tipoRelacionRepositorio)
        {
            PersonaRepositorio = _personaRepositorio;
            PersonaContactoRepositorio = _personaContactoRepositorio;
            PersonaRelacionRepositorio = _personaRelacionRepositorio;
            TipoRelacionRepositorio = _tipoRelacionRepositorio;
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
    
        public async Task<dtoPersona> GuardarPersonaAsync(Entidades.Persona personaAGuardar)
        {
            try
            {
                await ValidarDatosObligatoriosPersona(personaAGuardar);

                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {                    
                    Entidades.Persona persona = new Entidades.Persona();
                    personaAGuardar.Pais = null;
                    personaAGuardar.Sexo = null;
                    personaAGuardar.TipoDocumento = null;

                    if (personaAGuardar.Id == 0)
                    {
                        persona = await PersonaRepositorio.InsertarAsync(personaAGuardar);
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

                        foreach (PersonaContacto contacto in personaAGuardar.PersonaContacto)
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

                        for (int i = 0; i < persona.PersonaContacto.Count; i++)
                        {
                            if (!personaAGuardar.PersonaContacto.Exists(p => p.Id == persona.PersonaContacto[i].Id))
                            {
                                await PersonaContactoRepositorio.EliminarAsync(persona.PersonaContacto[i]);
                            }
                        }

                        await PersonaRepositorio.ActualizarAsync(persona);
                    }

                    dtoPersona dtoPersona = await ObtenerPersonaAsync(persona.Id);

                    transaction.Complete();

                    return dtoPersona;
                }
            }
            catch (AccesoADatosException ex)
            {
                DatosInvalidosException datosInvalidos = new DatosInvalidosException();

                if (ex.InnerException.InnerException.Message.Contains("FK_Persona_Pais"))
                {
                    datosInvalidos.Data.Add("Pais invalido", "El pais no se encuentra configurado.");
                }
                else if (ex.InnerException.InnerException.Message.Contains("FK_Persona_Sexo"))
                {
                    datosInvalidos.Data.Add("Sexo invalido", "El sexo no se encuentra configurado.");
                }
                else if (ex.InnerException.InnerException.Message.Contains("FK_Persona_TipoDocumento"))
                {
                    datosInvalidos.Data.Add("Tipo Documento invalido", "El tipo de documento no se encuentra configurado.");
                }

                if (datosInvalidos.Data.Count > 0)
                {
                    throw datosInvalidos;
                }
                else
                {
                    throw new Exception();
                }
            }
        }

        public async Task EliminarPersonaAsync(int id)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                Entidades.Persona persona = await ObtenerPersonaPrivadoAsync(id);
                foreach (PersonaContacto item in persona.PersonaContacto)
                {
                    await PersonaContactoRepositorio.EliminarAsync(item);
                }

                List<PersonaRelacion> relacionesPersona = await ObtenerRelacionesDePersonaAsync(id);
                foreach (PersonaRelacion relacion in relacionesPersona)
                {
                    await PersonaRelacionRepositorio.EliminarAsync(relacion);
                }
                await PersonaRepositorio.EliminarAsync(persona);

                transaction.Complete();
            }
        }

        public async Task<dtoEstadisticas> ObtenerEstadisticasAsync()
        {
            int cantidadMujeres = 0, cantidadHombres = 0, cantidadArgentinos = 0, totalPersonas = 0;

            List<Entidades.Persona> personas = await PersonaRepositorio.ObtenerListadoAsync();

            totalPersonas = personas.Count();

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
                PorcentajeArgentinos = Math.Round((double)cantidadArgentinos * 100 / (totalPersonas == 0 ? 1 : totalPersonas),2)
            };
        }

        public async Task<dtoPersonaRelacion> GuardarRelacionPadreAsync(int idPersona1, int idPersona2)
        {
            if (idPersona1 == idPersona2)
            {
                DatosInvalidosException datosInvalidos = new DatosInvalidosException();
                datosInvalidos.Data.Add("Relacion Invalida", "Una persona no puede ser padre de si mismo");
                throw datosInvalidos;
            }

            PersonaRelacion personaRelacion = await ObtenerPersonaRelacionPrivadoAsync(idPersona1, idPersona2);
            if (personaRelacion == null)
            {
                personaRelacion = await ObtenerPersonaRelacionPrivadoAsync(idPersona2, idPersona1);
                if (personaRelacion == null)
                {
                    personaRelacion = await PersonaRelacionRepositorio.InsertarAsync(new PersonaRelacion() { IdPersona1 = idPersona1, IdPersona2 = idPersona2, IdTipoRelacion = (byte)Entidades.Enumeraciones.TipoRelacionEnum.Padre });
                }
                else
                {
                    DatosInvalidosException datosInvalidos = new DatosInvalidosException();
                    datosInvalidos.Data.Add("Relacion Existente", "La persona " + idPersona1 + " ya se tiene una relacion registrada de " +  personaRelacion.TipoRelacion.Descripcion + "con la persona " + idPersona2);
                    throw datosInvalidos;    
                }
            }
            else
            {
                if (personaRelacion.IdTipoRelacion != (byte)Entidades.Enumeraciones.TipoRelacionEnum.Padre)
                {
                    DatosInvalidosException datosInvalidos = new DatosInvalidosException();
                    datosInvalidos.Data.Add("Relacion Existente", "La persona " + idPersona1 + " ya se tiene una relacion registrada de " + personaRelacion.TipoRelacion.Descripcion + "con la persona " + idPersona2);
                    throw datosInvalidos;
                }
            }

            TipoRelacion tipoRelacion = await TipoRelacionRepositorio.ObtenerUnoAsync(tr => tr.Id == personaRelacion.IdTipoRelacion);
            return new dtoPersonaRelacion() { IdPersona1 = idPersona1, IdPersona2 = idPersona2, IdPersonaRelacion = personaRelacion.Id, Relacion = tipoRelacion.Descripcion };
        }

        public async Task<dtoTipoRelacion> ObtenerRelacionAsync(int idPersona1, int idPersona2)
        {
            PersonaRelacion personaRelacion = await ObtenerPersonaRelacionPrivadoAsync(idPersona1, idPersona2);
            if (personaRelacion == null)
            {
                return null;
            }
            return new dtoTipoRelacion() { Relacion = personaRelacion.TipoRelacion.Descripcion };
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

        private async Task<Entidades.Persona> ObtenerPersonaPrivadoAsync(int id)
        {
            return await PersonaRepositorio.ObtenerUnoAsync(p => p.Id == id, p => p.Pais, p => p.TipoDocumento, p => p.PersonaContacto, p => p.Sexo);           
        }

        private async Task<PersonaRelacion> ObtenerPersonaRelacionPrivadoAsync(int idPersona1, int idPersona2)
        {
            return await PersonaRelacionRepositorio.ObtenerUnoAsync(r => r.IdPersona1 == idPersona1 && r.IdPersona2 == idPersona2, r => r.TipoRelacion);
        }

        private async Task<List<PersonaRelacion>> ObtenerRelacionesDePersonaAsync(int id)
        {
            return await PersonaRelacionRepositorio.ObtenerListadoAsync(r => r.IdPersona1 == id || r.IdPersona2 == id);
        }
    }
}
