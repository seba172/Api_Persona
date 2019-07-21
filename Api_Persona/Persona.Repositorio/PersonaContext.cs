using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persona.Repositorio
{
    public class PersonaContext : DbContext
    {
        public PersonaContext(DbContextOptions<PersonaContext> options) : base(options) { }

        public DbSet<Entidades.Pais> Pais { get; set; }
        public DbSet<Entidades.Persona> Persona { get; set; }
        public DbSet<Entidades.PersonaContacto> PersonaContacto { get; set; }
        public DbSet<Entidades.TipoDocumento> TipoDocumento { get; set; }
    }
}
