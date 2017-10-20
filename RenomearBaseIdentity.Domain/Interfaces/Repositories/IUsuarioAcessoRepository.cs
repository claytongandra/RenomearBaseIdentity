using System;
using RenomearBaseIdentity.Domain.Entities;

namespace RenomearBaseIdentity.Domain.Interfaces.Repositories
{
    public interface IUsuarioAcessoRepository : IDisposable
    {
        UsuarioAcesso GetAcessoByUsuarioId(string id);
        string GetLoginByEmailOrUser(string login);
    }
}
