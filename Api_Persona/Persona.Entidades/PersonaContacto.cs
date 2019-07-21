using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Persona.Entidades
{
    public class PersonaContacto
    {
        [Key]
        public int Id { get; set; }
        public int IdPersona { get; set; }
        public string Valor { get; set; }

        [ForeignKey("IdPersona")]
        public Persona Persona { get; set; }
    }
}
