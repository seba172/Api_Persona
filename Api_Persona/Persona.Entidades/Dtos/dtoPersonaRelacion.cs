using System;
using System.Collections.Generic;
using System.Text;

namespace Persona.Entidades.Dtos
{
    public class DtoPersonaRelacion
    {
        public int IdPersona1 { get; set; }
        public int IdPersona2 { get; set; }
        public string Relacion { get; set; }
    }
}
