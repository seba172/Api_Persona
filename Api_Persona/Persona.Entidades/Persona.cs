using System;
using System.Collections.Generic;
using System.Text;

namespace Persona.Entidades
{
    public class Persona
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string NumeroDocumento { get; set; }
        public byte IdTipoDocumento { get; set; }
        public Int16 IdPais { get; set; }
        public string Sexo { get; set; }
        public DateTime FechaNacimiento { get; set; }

        public TipoDocumento TipoDocumento { get; set; }
        public Pais Pais { get; set; }

        public List<TipoContacto> TipoContacto { get; set; }

    }
}
