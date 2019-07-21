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
        [MaxLength(50, ErrorMessage = "Máximo 50 caracteres")]
        public string Nombre { get; set; }
        [MaxLength(50, ErrorMessage = "Máximo 50 caracteres")]
        public string Apellido { get; set; }
        [MaxLength(12, ErrorMessage = "Máximo 12 caracteres")]
        [Required]
        public string NumeroDocumento { get; set; }
        [Required]
        public byte IdTipoDocumento { get; set; }
        [Required]    
        public Int16 IdPais { get; set; }
        [Required]
        public string Sexo { get; set; }
        [Required]
        public DateTime FechaNacimiento { get; set; }

        [ForeignKey("IdTipoDocumento")]
        public TipoDocumento TipoDocumento { get; set; }
        [ForeignKey("IdPais")]
        public Pais Pais { get; set; }

        public List<PersonaContacto> PersonaContacto { get; set; }
    }
}
