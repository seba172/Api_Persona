using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Persona.Entidades
{
    public class TipoDocumento
    {
        [Key]
        public int Id { get; set; }
        public string Descripcion { get; set; }
    }
}
