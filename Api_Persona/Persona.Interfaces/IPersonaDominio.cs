using Persona.Entidades.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Persona.Interfaces
{
    public interface IPersonaDominio
    {
        Task<DtoPersona> ObtenerPersonaAsync(int id);
        Task<List<DtoPersona>> ObtenerListadoPersonasAsync();
        Task<DtoPersona> InsertarPersonaAsync(Entidades.Persona personaAGuardar);
        Task<DtoPersona> ActualizarPersonaAsync(int id, Entidades.Persona personaAGuardar);
        Task EliminarPersonaAsync(int id);
        Task<DtoEstadisticas> ObtenerEstadisticasAsync();
        Task<DtoPersonaRelacion> GuardarRelacionPadreAsync(int idPersona1, int idPersona2);
        Task<DtoTipoRelacion> ObtenerRelacionAsync(int idPersona1, int idPersona2);
    }
}
