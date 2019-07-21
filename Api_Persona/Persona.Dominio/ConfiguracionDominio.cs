using Persona.Entidades;
using Persona.Entidades.Dtos;
using Persona.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persona.Dominio
{
    public class ConfiguracionDominio : IConfiguracionDominio
    {
        readonly IRepositorio<Pais> PaisRepositorio;
        readonly IRepositorio<TipoDocumento> TipoDocumentoRepositorio;

        public ConfiguracionDominio(IRepositorio<Pais> _paisRepositorio, IRepositorio<TipoDocumento> _tipoDocumentoRepositorio)
        {
            PaisRepositorio = _paisRepositorio;
            TipoDocumentoRepositorio = _tipoDocumentoRepositorio;
        }

        public async Task<Pais> ObtenerPaisAsync(int id)
        {
            return await PaisRepositorio.ObtenerPorIDAsync(id);
        }

        public async Task<TipoDocumento> ObtenerTipoDocumentoAsync(int id)
        {
            return await TipoDocumentoRepositorio.ObtenerPorIDAsync(id);
        }
        
        public async Task<List<dtoPais>> ObtenerPaisesAsync()
        {
            List<Pais> paises = await PaisRepositorio.ObtenerListadoAsync();

            var _paisesDto = (from p in paises
                                    select new dtoPais
                                    {
                                        Id = p.Id,
                                        Descripcion = p.Descripcion
                                    }).ToList();

            return _paisesDto;
        }

        public async Task<List<dtoTipoDocumento>> ObtenerTipoDocumentosAsync()
        {
            List<TipoDocumento> tipoDocumentos = await TipoDocumentoRepositorio.ObtenerListadoAsync();

            var _tipoDocumentosDto = (from p in tipoDocumentos
                                            select new dtoTipoDocumento
                                            {
                                                Id = p.Id,
                                                Descripcion = p.Descripcion
                                            }).ToList();

            return _tipoDocumentosDto;
        }      
    }
}
