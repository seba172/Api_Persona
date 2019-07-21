using Persona.Entidades;
using Persona.Entidades.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Persona.Interfaces
{
    public interface IConfiguracionDominio
    {
        Task<List<dtoPais>> ObtenerPaisesAsync();
        Task<List<dtoTipoDocumento>> ObtenerTipoDocumentosAsync();
        Task<Pais> ObtenerPaisAsync(int id);
        Task<TipoDocumento> ObtenerTipoDocumentoAsync(int id);
    }
}
