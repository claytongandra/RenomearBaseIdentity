
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenomearBaseIdentity.Domain.Entities;
using RenomearBaseIdentity.Domain.Interfaces.Repositories;

namespace RenomearBaseIdentity.Infra.Data.Repositories
{
    public class UsuarioAcessoRepository : Repository<UsuarioAcesso>, IUsuarioAcessoRepository
    {

        public UsuarioAcessoRepository(UsuarioContext context)
            : base(context)
        {

        }

        public UsuarioAcesso GetAcessoByUsuarioId(string id)
        {
            return DbSet.Find(id);
        }

        public string GetLoginByEmailOrUser(string login)
        {
            UsuarioAcesso retornoQueryUser = (from UserLogin in DbSet
                                              where (UserLogin.UsuarioAcesso_Email == login || UserLogin.UsuarioAcesso_UserName == login)
                                              select UserLogin).SingleOrDefault();

            if (retornoQueryUser == null)
            {
                return null;
            }

            return retornoQueryUser.UsuarioAcesso_UserName;
        }
        //public void Dispose()
        //{
        //    DbSet.Dispose();
        //}
    }
}
