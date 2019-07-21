using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Persona.Entidades
{
    public class Pais
    {
        [Key]
        public Int16 Id { get; set; }
        public string Descripcion { get; set; }
    }
}
