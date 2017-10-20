using System;

namespace RenomearBaseIdentity.Domain.Entities
{
    public class UsuarioAcesso
    {
        public string UsuarioAcesso_Id { get; set; }
        public string UsuarioAcesso_UserName { get; set; }
        public string UsuarioAcesso_Email { get; set; }
        public bool UsuarioAcesso_EmailConfirmed { get; set; }
        public string UsuarioAcesso_PasswordHash { get; set; }
        public string UsuarioAcesso_SecurityStamp { get; set; }
        public string UsuarioAcesso_PhoneNumber { get; set; }
        public bool UsuarioAcesso_PhoneNumberConfirmed { get; set; }
        public bool UsuarioAcesso_TwoFactorEnabled { get; set; }
        public DateTime? UsuarioAcesso_LockoutEndDateUtc { get; set; }
        public bool UsuarioAcesso_LockoutEnabled { get; set; }
        public int UsuarioAcesso_AccessFailedCount { get; set; }
        public int UsuarioAcesso_Nivel { get; set; } //10 - Admin geral, 20 - Admin Conteudos, 30 - Autor, 50 - Usuario
        public string UsuarioAcesso_CodigoUsuario { get; set; }
        public virtual Usuario UsuarioAcesso_Usuario { get; set; }
    }
}
