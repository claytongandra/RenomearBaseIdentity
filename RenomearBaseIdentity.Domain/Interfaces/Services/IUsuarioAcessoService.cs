using RenomearBaseIdentity.Domain.Entities;

namespace RenomearBaseIdentity.Domain.Interfaces.Services
{
    public interface IUsuarioAcessoService
    {
        UsuarioAcesso GetAcessoByUsuarioId(string id);
        string GetLoginByEmailOrUser(string login);
    }
}
