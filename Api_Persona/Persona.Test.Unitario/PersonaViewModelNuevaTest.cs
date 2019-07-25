using Persona.Api;
using Persona.Api.Models;
using Persona.Api.Validadores;
using Persona.Entidades.Enumeraciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;

namespace Persona.TestUnitario.Models
{
    public class PersonaViewModelNuevaTest
    {
        [Fact]
        public void PersonaViewModelNueva_Validar_Datos()
        {
            // Inicializacion
            List<ValidationResult> validaciones = new List<ValidationResult>();
            PersonaViewModelNueva personaVM = new PersonaViewModelNueva();
            ValidationContext validacionContexto = new ValidationContext(personaVM, null, null);

            // Acto
            bool valido = Validator.TryValidateObject(personaVM, validacionContexto, validaciones);

            // Acierto
            Assert.False(valido);
            Assert.Contains(validaciones, p => p.ErrorMessage.Contains("Debe ingresar NumeroDocumento"));
            Assert.Contains(validaciones, p => p.ErrorMessage.Contains("Debe ingresar Nombre"));
            Assert.Contains(validaciones, p => p.ErrorMessage.Contains("Debe ingresar Apellido"));
            Assert.Contains(validaciones, p => p.ErrorMessage.Contains("Debe ingresar al menos un contacto"));

            Assert.Equal(4, validaciones.Count);
        }
    }
}
