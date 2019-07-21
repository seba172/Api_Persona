using System;
using System.Collections.Generic;
using System.Text;

namespace Persona.Entidades
{
    public class PersonaContacto
    {
        public int Id { get; set; }
        public int IdPersona { get; set; }
        public int IdTipoContacto { get; set; }
        public int Valor { get; set; }

        public Persona Persona { get; set; }
        public TipoContacto TipoContacto { get; set; }
    }
}
