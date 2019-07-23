using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Persona.Entidades
{
    public class PersonaRelacion
    {
        [Key]
        public int Id { get; set; }
        public int IdPersona1 { get; set; }
        public int IdPersona2 { get; set; }
        public byte IdTipoRelacion { get; set; }

        [ForeignKey("IdTipoRelacion")]
        public TipoRelacion TipoRelacion { get; set; }
    }
}
