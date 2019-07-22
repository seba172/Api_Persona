using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Persona.Entidades
{
    public class Sexo
    {
        [Key]
        public byte Id { get; set; }
        public string Descripcion { get; set; }
    }
}
