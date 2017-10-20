using System.Data.Entity;

namespace RenomearBaseIdentity.Infra.Data
{
    public class RenomearBaseIdentityContext : DbContext
    {
        public RenomearBaseIdentityContext()
        :base("RenomearBaseIdentity") //Nome da Base de Dados
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
