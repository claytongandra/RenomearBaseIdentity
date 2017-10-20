using RenomearBaseIdentity.Application.Interfaces;
using RenomearBaseIdentity.Domain.Entities;
using RenomearBaseIdentity.Domain.Interfaces.Services;

namespace RenomearBaseIdentity.Application.Services
{
    public class UsuarioAcessoAppService : IUsuarioAcessoAppService
    {
        private readonly IUsuarioAcessoService _usuarioAcessoService;

        public UsuarioAcessoAppService(IUsuarioAcessoService usuarioAcessoService)
        {
            _usuarioAcessoService = usuarioAcessoService;
        }

        public UsuarioAcesso GetAcessoByUsuarioId(string id)
        {
            return _usuarioAcessoService.GetAcessoByUsuarioId(id);
        }

        public string GetLoginByEmailOrUser(string login)
        {
            return _usuarioAcessoService.GetLoginByEmailOrUser(login);
        }
    }
}
