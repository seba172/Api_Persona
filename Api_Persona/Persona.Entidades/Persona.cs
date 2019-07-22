using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Persona.Entidades
{
    public class Persona
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public string Nombre { get; set; }
        [MaxLength(50)]
        public string Apellido { get; set; }
        [MaxLength(12)]
        [Required]
        public string NumeroDocumento { get; set; }
        [Required]
        public byte IdTipoDocumento { get; set; }
        [Required]    
        public Int16 IdPais { get; set; }
        [Required]
        public byte IdSexo { get; set; }
        [Required]
        public DateTime FechaNacimiento { get; set; }

        [ForeignKey("IdTipoDocumento")]
        public TipoDocumento TipoDocumento { get; set; }
        [ForeignKey("IdPais")]
        public Pais Pais { get; set; }
        [ForeignKey("IdSexo")]
        public Sexo Sexo { get; set; }

        public List<PersonaContacto> PersonaContacto { get; set; }
    }
}
