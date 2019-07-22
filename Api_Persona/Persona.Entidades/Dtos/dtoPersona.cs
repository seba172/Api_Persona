using System;
using System.Collections.Generic;
using System.Text;

namespace Persona.Entidades.Dtos
{
    public class dtoPersona
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string NumeroDocumento { get; set; }
        public byte? IdTipoDocumento { get; set; }
        public Int16? IdPais { get; set; }
        public byte? IdSexo { get; set; }
        public DateTime? FechaNacimiento { get; set; }

        public List<dtoContacto> PersonaContacto { get; set; }

        public string Pais { get; set; }
        public string TipoDocumento { get; set; }
        public string Sexo { get; set; }

    }
}
