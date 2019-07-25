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
        public void Persona1_Tiene_Relacion_Persona2()
        {
            var personaRelacionRepositorio = new Mock<IRepositorio<PersonaRelacion>>();
            personaRelacionRepositorio.Setup(p => p.ObtenerUnoAsync(It.IsAny<Expression<Func<PersonaRelacion,Boolean>>>(), null)).Returns(ObtenerUnaRelacionEntrePersonasAsync());

            var personaDominio = new PersonaDominio(null, null, personaRelacionRepositorio.Object, null);

            int idPersona1 = 1;
            int idPersona2 = 2;
            var relacion = personaDominio.ObtenerRelacionAsync(idPersona1, idPersona2);

            Assert.Equal(idPersona1 + " es " + TipoRelacionEnum.Padre + " de " + idPersona2, relacion.Result.Relacion);
        }

        public async Task<PersonaRelacion> ObtenerUnaRelacionEntrePersonasAsync()
        {
            return new PersonaRelacion { Id = 1, IdPersona1 = 1, IdPersona2 = 2, IdTipoRelacion = (int)TipoRelacionEnum.Padre };
        }
    }
}
