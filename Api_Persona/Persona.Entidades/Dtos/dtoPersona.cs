using System;
using System.Collections.Generic;
using System.Text;

namespace Persona.Entidades.Dtos
{
    public class DtoPersona
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string NumeroDocumento { get; set; }
        public DateTime? FechaNacimiento { get; set; }

        public List<DtoContacto> PersonaContacto { get; set; }

        public string Pais { get; set; }
        public string TipoDocumento { get; set; }
        public string Sexo { get; set; }

    }
}
