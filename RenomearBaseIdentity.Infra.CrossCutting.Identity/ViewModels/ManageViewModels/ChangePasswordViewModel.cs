using System.ComponentModel.DataAnnotations;

namespace RenomearBaseIdentity.Infra.CrossCutting.Identity.ViewModels.ManageViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Preencha o campo Senha atual.")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha atual")]
        public string UsuarioAcesso_OldPassword { get; set; }

        [Required(ErrorMessage = "Preencha o campo Nova senha.")]
        [StringLength(100, ErrorMessage = "A {0} deve conter pelo menos {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nova senha")]
        public string UsuarioAcesso_NewPassword { get; set; }

        [Required(ErrorMessage = "Preencha o campo Confirmação da Senha.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmação da Senha")]
        [Compare("UsuarioAcesso_NewPassword", ErrorMessage = "A senha e a confirmação da senha estão diferentes.")]
        public string UsuarioAcesso_ConfirmPassword { get; set; }
    }
}
