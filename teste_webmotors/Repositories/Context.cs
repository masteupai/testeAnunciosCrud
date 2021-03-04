using Microsoft.EntityFrameworkCore;
using teste_webmotors.Model;
using teste_webmotors.Models.Mappings;

namespace teste_webmotors.Repositories
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        { 
        }
        public DbSet<Anuncio> Anuncios { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AnuncioMapping).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
