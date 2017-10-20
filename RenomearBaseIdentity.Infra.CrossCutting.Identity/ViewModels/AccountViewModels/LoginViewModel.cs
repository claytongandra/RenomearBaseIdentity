using System.ComponentModel.DataAnnotations;

namespace RenomearBaseIdentity.Infra.CrossCutting.Identity.ViewModels.AccountViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Informe seu Usuário ou E-mail.")]
        [Display(Name = "Usuário ou Email")]
        public string UsuarioAcesso_UserName { get; set; }

        [Required(ErrorMessage = "Informe sua senha.")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string UsuarioAcesso_Password { get; set; }

        [Display(Name = "Mantenha-me conectado.")]
        public bool UsuarioAcesso_RememberMe { get; set; }

    }
}
