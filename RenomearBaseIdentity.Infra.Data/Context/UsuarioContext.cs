using System.Data.Entity;
using RenomearBaseIdentity.Domain.Entities;
using RenomearBaseIdentity.Infra.Data.EntitiesConfig;

namespace RenomearBaseIdentity.Infra.Data
{
    public class UsuarioContext : DbContext
    {
        public UsuarioContext()
            : base("RenomearBaseIdentity")
        {

        }
        public DbSet<Usuario> NewLearningCloud_Usuario { get; set; }
        public DbSet<UsuarioAcesso> NewLearningCloud_UsuarioAcesso { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UsuarioConfiguration());
            modelBuilder.Configurations.Add(new UsuarioAcessoConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
