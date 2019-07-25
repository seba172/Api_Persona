using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Persona.Api.Validadores;
using Persona.Entidades.Enumeraciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Persona.Api.Models
{
    public class PersonaViewModelNueva : IValidatableObject
    {
        [Required(ErrorMessage = "Debe ingresar {0}")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Debe ingresar {0}")]
        public string Apellido { get; set; }
        [Required(ErrorMessage = "Debe ingresar {0}")]
        [RegularExpression(@"^[0-9]{5,12}$", ErrorMessage = "{0} debe ser numerico de 5 a 12 caracteres")]
        public string NumeroDocumento { get; set; }
        [Required(ErrorMessage = "Debe ingresar {0}")]
        public TipoDocumentoEnum TipoDoc { get; set; }
        [Required(ErrorMessage = "Debe ingresar {0}")]
        public PaisEnum PaisPersona { get; set; }
        [Required(ErrorMessage = "Debe ingresar {0}")]
        public SexoEnum SexoPersona { get; set; }
        [Required(ErrorMessage = "Debe ingresar {0}")]
        public DateTime FechaNacimiento { get; set; }
        [Required(ErrorMessage = "Debe ingresar al menos un contacto")]
        [ListaConUnElementoAtributo(ErrorMessage = "Debe ingresar al menos un contacto")]
        public List<string> Contactos { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FechaNacimiento.AddYears(18) > DateTime.Now)
            {
                yield return new ValidationResult(
                    $"La persona debe ser mayor a 18 años.",
                    new[] { "FechaNacimiento" });
            }
        }
    }
}
