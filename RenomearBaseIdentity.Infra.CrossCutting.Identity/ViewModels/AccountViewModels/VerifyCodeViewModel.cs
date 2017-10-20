using System.ComponentModel.DataAnnotations;

namespace RenomearBaseIdentity.Infra.CrossCutting.Identity.ViewModels.AccountViewModels
{
    public class VerifyCodeViewModel
    {
        [Required(ErrorMessage = "Problemas ao recuperar o provedor. Atualize a página e tente novamente.")]
        public string Provider { get; set; }

        [Required(ErrorMessage = "Preencha o campo Código.")]
        [Display(Name = "Código")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Lembrar desse navegador?")]
        public bool RememberBrowser { get; set; }

        [ScaffoldColumn(false)]
        public bool RememberMe { get; set; }
    }
}
