using Persona.Entidades.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Persona.Interfaces
{
    public interface IPersonaDominio
    {
        Task<dtoPersona> ObtenerPersonaAsync(int id);
        Task<List<dtoPersona>> ObtenerListadoPersonasAsync();
        Task<dtoPersona> GuardarPersonaAsync(Entidades.Persona Persona);
        Task EliminarPersonaAsync(int id);
        Task<dtoEstadisticas> ObtenerEstadisticasAsync();
    }
}
