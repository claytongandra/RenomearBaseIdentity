using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RenomearBaseIdentity.Domain.Entities;

namespace RenomearBaseIdentity.Infra.CrossCutting.Identity.ViewModels
{
    public class ApplicationUser : IdentityUser<string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            userIdentity.AddClaims(new[] {
                new Claim("FullName", UsuarioAcesso_Usuario.Usuario_Nome + " " + UsuarioAcesso_Usuario.Usuario_SobreNome),
                new Claim("UserId",UsuarioAcesso_Usuario.Usuario_Id),
                new Claim("UserAccessId", Id),
                new Claim("Nivel", UsuarioAcesso_Nivel.ToString())
            });
            return userIdentity;
        }
        public int UsuarioAcesso_Nivel { get; set; }
        public virtual Usuario UsuarioAcesso_Usuario { get; set; }
    }
}
