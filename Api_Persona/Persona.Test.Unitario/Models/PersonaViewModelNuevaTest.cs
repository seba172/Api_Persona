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

namespace Persona.Test.Unitario.Models
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
            bool valido = Validator.TryValidateObject(personaVM, validacionContexto, validaciones, true);

            // Acierto
            Assert.False(valido);
            Assert.Contains(validaciones, p => p.ErrorMessage.Contains("Debe ingresar NumeroDocumento"));
            Assert.Contains(validaciones, p => p.ErrorMessage.Contains("Debe ingresar Nombre"));
            Assert.Contains(validaciones, p => p.ErrorMessage.Contains("Debe ingresar Apellido"));
            Assert.Contains(validaciones, p => p.ErrorMessage.Contains("Debe ingresar al menos un contacto"));

            Assert.Equal(4, validaciones.Count);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("178")]
        [InlineData("17896547125654")]
        [InlineData("4578787A8745")]
        public void PersonaViewModelNueva_Validar_Documento(string numeroDocumento)
        {
            // Inicializacion
            List<ValidationResult> validaciones = new List<ValidationResult>();
            PersonaViewModelNueva personaVM = new PersonaViewModelNueva();
            personaVM.Apellido = "Fernandez";
            personaVM.Nombre = "Sebastian";
            personaVM.Contactos = new List<string>() { "contacto" };
            personaVM.FechaNacimiento = DateTime.Now.AddYears(-20);
            personaVM.NumeroDocumento = numeroDocumento;

            ValidationContext validacionContexto = new ValidationContext(personaVM, null, null);

            // Acto
            bool valido = Validator.TryValidateObject(personaVM, validacionContexto, validaciones, true);

            // Acierto
            Assert.False(valido);
            Assert.Single(validaciones);
        }
    }
}
