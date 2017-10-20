using System.ComponentModel.DataAnnotations;

namespace RenomearBaseIdentity.Infra.CrossCutting.Identity.ViewModels.AccountViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Preencha o campo Email.")]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string UsuarioAcesso_Email { get; set; }

        [Required(ErrorMessage = "Preencha o campo Senha.")]
        [StringLength(100, ErrorMessage = "A {0} deve conter pelo menos {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string UsuarioAcesso_Password { get; set; }

        [Required(ErrorMessage = "Preencha o campo Confirmação da Senha.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmação da Senha")]
        [Compare("UsuarioAcesso_Password", ErrorMessage = "A senha e a confirmação da senha estão diferentes.")]
        public string UsuarioAcesso_ConfirmPassword { get; set; }

        [ScaffoldColumn(false)]
        //[System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public string Code { get; set; }
    }
}
