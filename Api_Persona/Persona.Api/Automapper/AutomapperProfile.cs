using AutoMapper;
using Persona.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persona.Api.Automapper
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<PersonaViewModelNueva, Entidades.Persona>()
                .ForMember(dest => dest.IdPais, act => act.MapFrom(src => src.PaisPersona))
                .ForMember(dest => dest.IdSexo, act => act.MapFrom(src => src.SexoPersona))
                .ForMember(dest => dest.IdTipoDocumento, act => act.MapFrom(src => src.TipoDoc))
                .ForMember(dest => dest.PersonaContacto, act => act.MapFrom(j => j.Contactos.Select(k => new Entidades.PersonaContacto { Valor = k }).ToList()));

            CreateMap<PersonaViewModelActualizar, Entidades.Persona>()
                .ForMember(dest => dest.IdPais, act => act.MapFrom(src => src.PaisPersona))
                .ForMember(dest => dest.IdSexo, act => act.MapFrom(src => src.SexoPersona))
                .ForMember(dest => dest.IdTipoDocumento, act => act.MapFrom(src => src.TipoDoc))
                .ForMember(dest => dest.PersonaContacto, act => act.MapFrom(j => j.Contactos.Select(k => new Entidades.PersonaContacto { Valor = k }).ToList()));
        }
    }
}
