using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Persona.Entidades.Enumeraciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Persona.Api.Models
{
    public class PersonaViewModelNueva
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string NumeroDocumento { get; set; }
        public TipoDocumentoEnum TipoDoc { get; set; }
        public PaisEnum PaisPersona { get; set; }
        public SexoEnum SexoPersona { get; set; }
        public DateTime FechaNacimiento { get; set; }

        public List<string> Contactos { get; set; }
    }
}
