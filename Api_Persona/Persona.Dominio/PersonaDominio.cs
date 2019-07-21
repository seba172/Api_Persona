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
        readonly IConfiguracionDominio ConfiguracionDominio;

        public PersonaDominio(IRepositorio<Entidades.Persona> _personaRepositorio, IConfiguracionDominio _configuracionDominio)
        {
            PersonaRepositorio = _personaRepositorio;
            ConfiguracionDominio = _configuracionDominio;
        }

        public async Task EliminarPersonaAsync(int id)
        {
            Entidades.Persona persona = await ObtenerPersonaPrivadoAsync(id);
            await PersonaRepositorio.EliminarAsync(persona);
        }

        public async Task<Entidades.Persona> GuardarPersonaAsync(dtoPersona dtoPersona)
        {
            if (dtoPersona.Id == 0)
            {
                Entidades.Persona persona = new Entidades.Persona();
                persona.Nombre = dtoPersona.Nombre;
                persona.Apellido = dtoPersona.Apellido;
                persona.FechaNacimiento = dtoPersona.FechaNacimiento.Value;
                persona.IdTipoDocumento = dtoPersona.IdTipoDocumento.Value;
                persona.IdPais = dtoPersona.IdPais.Value;
                persona.NumeroDocumento = dtoPersona.NumeroDocumento;
                persona.Sexo = dtoPersona.Sexo;

                await ValidarDatosPersona(persona);

                return await PersonaRepositorio.InsertarAsync(persona);
            }
            else
            {
                Entidades.Persona persona = await ObtenerPersonaPrivadoAsync(dtoPersona.Id);
                persona.Nombre = string.IsNullOrEmpty(dtoPersona.Nombre) ? persona.Nombre : dtoPersona.Nombre;
                persona.Apellido = string.IsNullOrEmpty(dtoPersona.Apellido) ? persona.Apellido : dtoPersona.Apellido;
                persona.FechaNacimiento = dtoPersona.FechaNacimiento ?? persona.FechaNacimiento;
                persona.IdTipoDocumento = dtoPersona.IdTipoDocumento ?? persona.IdTipoDocumento;
                persona.IdPais = dtoPersona.IdPais ?? persona.IdPais;
                persona.NumeroDocumento = string.IsNullOrEmpty(dtoPersona.NumeroDocumento) ? persona.NumeroDocumento : dtoPersona.NumeroDocumento;
                persona.Sexo = string.IsNullOrEmpty(dtoPersona.Sexo) ? persona.Sexo : dtoPersona.Sexo;

                await PersonaRepositorio.ActualizarAsync(persona);

                return persona;
            }
        }

        public async Task<List<dtoPersona>> ObtenerListadoPersonasAsync()
        {
            List<Entidades.Persona> personas = await PersonaRepositorio.ObtenerListadoAsync(p => p.Pais, p => p.TipoDocumento, p => p.PersonaContacto);

            var dtoPersonas = (from persona in personas
                               select new dtoPersona
                               {
                                   Apellido = persona.Apellido,
                                   Nombre = persona.Nombre,
                                   Id = persona.Id,
                                   NumeroDocumento = persona.NumeroDocumento,
                                   FechaNacimiento = persona.FechaNacimiento,
                                   Sexo = persona.Sexo,
                                   TipoDocumento = persona.TipoDocumento.Descripcion,
                                   Pais = persona.Pais.Descripcion,
                                   IdTipoDocumento = persona.IdTipoDocumento,
                                   IdPais = persona.IdPais,
                                   Contactos = (from contactos in persona.PersonaContacto
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
            Entidades.Persona persona = await PersonaRepositorio.ObtenerPorIDAsync(id, p => p.Pais, p => p.TipoDocumento, p => p.PersonaContacto);

            return new dtoPersona() {
                 Apellido = persona.Apellido,
                 Nombre = persona.Nombre,
                 Id = persona.Id,
                 NumeroDocumento = persona.NumeroDocumento,
                 FechaNacimiento = persona.FechaNacimiento,
                 Sexo = persona.Sexo,
                 TipoDocumento = persona.TipoDocumento.Descripcion,
                 Pais = persona.Pais.Descripcion,
                 IdTipoDocumento = persona.IdTipoDocumento,
                 IdPais = persona.IdPais,
                 Contactos = (from contactos in persona.PersonaContacto
                              select new dtoContacto
                              {
                                  idContacto = contactos.Id,
                                  idPersona = contactos.IdPersona,
                                  Valor = contactos.Valor
                              }).ToList(),
            };
        }

        private async Task<bool> ValidarDatosPersona(Entidades.Persona persona)
        {
            Framework.Validaciones.Validaciones.Validar<Entidades.Persona>(persona);

            DatosInvalidosException datosInvalidos = null;

            if (persona.PersonaContacto.Count == 0)
            {
                datosInvalidos = new DatosInvalidosException();
                datosInvalidos.Data.Add("Persona sin contactos", "Debe indicar al menos un tipo de contacto");
            }

            if (persona.IdTipoDocumento > 0 && await ConfiguracionDominio.ObtenerTipoDocumentoAsync(persona.IdTipoDocumento) == null)
            {
                datosInvalidos = new DatosInvalidosException();
                datosInvalidos.Data.Add("Tipo Documento Inexistente", String.Format("El Id tipo de documento {0} no se encuentra configurado", persona.IdTipoDocumento));
            }

            if (persona.IdPais > 0 && await ConfiguracionDominio.ObtenerPaisAsync(persona.IdPais) == null)
            {
                datosInvalidos = new DatosInvalidosException();
                datosInvalidos.Data.Add("Pais Inexistente", String.Format("El Id pais {0} no se encuentra configurado", persona.IdTipoDocumento));
            }
           
            if (await PersonaRepositorio.ObtenerUnoAsync(p => p.IdPais == persona.IdPais && p.IdTipoDocumento == persona.IdTipoDocumento && p.Sexo == persona.Sexo && p.NumeroDocumento == persona.NumeroDocumento && p.Id != persona.Id) != null)
            {
                datosInvalidos = new DatosInvalidosException();
                datosInvalidos.Data.Add("Persona Repetida", "La persona ya Existe");
            }

            if (datosInvalidos == null)
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
            return await PersonaRepositorio.ObtenerPorIDAsync(id);
        }

    }
}
