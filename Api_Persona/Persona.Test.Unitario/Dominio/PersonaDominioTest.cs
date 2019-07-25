using Moq;
using Persona.Dominio;
using Persona.Entidades;
using Persona.Entidades.Enumeraciones;
using Persona.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Persona.Test.Unitario.Dominio
{
    public class PersonaDominioTest
    {
        [Fact]
        public void Persona1_EsPadre_De_Persona2()
        {
            var personaRelacionRepositorio = new Mock<IRepositorio<PersonaRelacion>>();
            personaRelacionRepositorio.Setup(p => p.ObtenerUnoAsync(It.IsAny<Expression<Func<PersonaRelacion,Boolean>>>(), It.IsAny<Expression<Func<PersonaRelacion, Object>>>())).Returns(ObtenerRelacionPadreHijoAsync());

            var personaDominio = new PersonaDominio(null, null, personaRelacionRepositorio.Object, null);

            int idPersona1 = 1;
            int idPersona2 = 2;

            var relacion = personaDominio.ObtenerRelacionAsync(idPersona1, idPersona2).Result;

            Assert.Equal((int)TipoRelacionEnum.Padre, relacion.IdTipoRelacion);
        }

        [Fact]
        public void Persona1_EsTio_De_Persona2()
        {
            var personaRelacionRepositorio = new Mock<IRepositorio<PersonaRelacion>>();
            personaRelacionRepositorio.Setup(p => p.ObtenerUnoAsync(It.IsAny<Expression<Func<PersonaRelacion, Boolean>>>(), It.IsAny<Expression<Func<PersonaRelacion, Object>>>())).Returns(ObtenerRelacionTioAsync());

            var personaDominio = new PersonaDominio(null, null, personaRelacionRepositorio.Object, null);

            int idPersona1 = 35;
            int idPersona2 = 25;

            var relacion = personaDominio.ObtenerRelacionAsync(idPersona1, idPersona2).Result;

            Assert.Equal((int)TipoRelacionEnum.Tio, relacion.IdTipoRelacion);
        }

        [Fact]
        public void Persona1_EsPrimo_De_Persona2()
        {
            var personaRelacionRepositorio = new Mock<IRepositorio<PersonaRelacion>>();
            personaRelacionRepositorio.Setup(p => p.ObtenerUnoAsync(It.IsAny<Expression<Func<PersonaRelacion, Boolean>>>(), It.IsAny<Expression<Func<PersonaRelacion, Object>>>())).Returns(ObtenerRelacionPrimoAsync());

            var personaDominio = new PersonaDominio(null, null, personaRelacionRepositorio.Object, null);

            int idPersona1 = 33;
            int idPersona2 = 45;

            var relacion = personaDominio.ObtenerRelacionAsync(idPersona1, idPersona2).Result;

            Assert.Equal((int)TipoRelacionEnum.Primo, relacion.IdTipoRelacion);
        }

        [Fact]
        public void Persona1_EsHermano_De_Persona2()
        {
            var personaRelacionRepositorio = new Mock<IRepositorio<PersonaRelacion>>();
            personaRelacionRepositorio.Setup(p => p.ObtenerUnoAsync(It.IsAny<Expression<Func<PersonaRelacion, Boolean>>>(), It.IsAny<Expression<Func<PersonaRelacion, Object>>>())).Returns(ObtenerRelacionHermanoAsync());

            var personaDominio = new PersonaDominio(null, null, personaRelacionRepositorio.Object, null);

            int idPersona1 = 10;
            int idPersona2 = 20;

            var relacion = personaDominio.ObtenerRelacionAsync(idPersona1, idPersona2).Result;

            Assert.Equal((int)TipoRelacionEnum.Hermano, relacion.IdTipoRelacion);
        }

        public async Task<PersonaRelacion> ObtenerRelacionPadreHijoAsync()
        {
            return new PersonaRelacion { Id = 1, IdPersona1 = 1, IdPersona2 = 2, IdTipoRelacion = (int)TipoRelacionEnum.Padre, TipoRelacion = new TipoRelacion { Id = (int)TipoRelacionEnum.Padre, Descripcion = "Padre"} };
        }

        public async Task<PersonaRelacion> ObtenerRelacionHermanoAsync()
        {
            return new PersonaRelacion { Id = 2, IdPersona1 = 10, IdPersona2 = 20, IdTipoRelacion = (int)TipoRelacionEnum.Hermano, TipoRelacion = new TipoRelacion { Id = (int)TipoRelacionEnum.Hermano, Descripcion = "Herman@" } };
        }

        public async Task<PersonaRelacion> ObtenerRelacionPrimoAsync()
        {
            return new PersonaRelacion { Id = 7, IdPersona1 = 33, IdPersona2 = 45, IdTipoRelacion = (int)TipoRelacionEnum.Primo, TipoRelacion = new TipoRelacion { Id = (int)TipoRelacionEnum.Primo, Descripcion = "Prim@" } };
        }

        public async Task<PersonaRelacion> ObtenerRelacionTioAsync()
        {
            return new PersonaRelacion { Id = 5, IdPersona1 = 35, IdPersona2 = 25, IdTipoRelacion = (int)TipoRelacionEnum.Tio, TipoRelacion = new TipoRelacion { Id = (int)TipoRelacionEnum.Tio, Descripcion = "Ti@" } };
        }
    }
}
