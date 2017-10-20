using RenomearBaseIdentity.Domain.Entities;
using RenomearBaseIdentity.Domain.Interfaces.Repositories;
using RenomearBaseIdentity.Domain.Interfaces.Services;

namespace RenomearBaseIdentity.Domain.Services
{
    public class UsuarioAcessoService : IUsuarioAcessoService
    {
        private readonly IUsuarioAcessoRepository _usuarioAcessoRepository;

        public UsuarioAcessoService(IUsuarioAcessoRepository usuarioAcessoRepository)
        {
            _usuarioAcessoRepository = usuarioAcessoRepository;
        }

        public UsuarioAcesso GetAcessoByUsuarioId(string id)
        {
            return _usuarioAcessoRepository.GetAcessoByUsuarioId(id);
        }

        public string GetLoginByEmailOrUser(string login)
        {
            return _usuarioAcessoRepository.GetLoginByEmailOrUser(login);
        }
    }
}
