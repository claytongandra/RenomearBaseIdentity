using System.ComponentModel.DataAnnotations;

namespace RenomearBaseIdentity.Infra.CrossCutting.Identity.ViewModels.AccountViewModels
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required(ErrorMessage = "Preencha o campo Usuário.")]
        [StringLength(100, ErrorMessage = "O {0} deve conter pelo menos {2} caracteres.", MinimumLength = 6)]
        [Display(Name = "Usuário")]
        public string UsuarioAcesso_UserName { get; set; }

        [Required(ErrorMessage = "Preencha o campo Nome.")]
        [StringLength(100, ErrorMessage = "O {0} deve conter pelo menos {2} caracteres.", MinimumLength = 3)]
        [Display(Name = "Nome")]
        public string Usuario_Nome { get; set; }

        [Required(ErrorMessage = "Preencha o campo Sobrenome.")]
        [Display(Name = "Sobrenome")]
        public string Usuario_SobreNome { get; set; }

        [Required(ErrorMessage = "Preencha o campo Email.")]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string UsuarioAcesso_Email { get; set; }
    }
}
