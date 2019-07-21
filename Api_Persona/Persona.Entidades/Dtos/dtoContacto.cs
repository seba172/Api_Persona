using System;
using System.Collections.Generic;
using System.Text;

namespace Persona.Entidades.Dtos
{
    public class dtoContacto
    {
        public int idContacto { get; set; }
        public int idPersona { get; set; }
        public string Valor { get; set; }
    }
}
