using RenomearBaseIdentity.Domain.Entities;

namespace RenomearBaseIdentity.Application.Interfaces
{
    public interface IUsuarioAcessoAppService
    {
        UsuarioAcesso GetAcessoByUsuarioId(string id);
        string GetLoginByEmailOrUser(string login);
    }
}
