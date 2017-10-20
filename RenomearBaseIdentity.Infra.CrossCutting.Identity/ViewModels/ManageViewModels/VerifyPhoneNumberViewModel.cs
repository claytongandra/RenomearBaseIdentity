using System.ComponentModel.DataAnnotations;

namespace RenomearBaseIdentity.Infra.CrossCutting.Identity.ViewModels.ManageViewModels
{
    public class VerifyPhoneNumberViewModel
    {
        [Required(ErrorMessage = "Preencha o campo Código.")]
        [Display(Name = "Código")]
        public string Code { get; set; }

        [ScaffoldColumn(false)]
        [Required(ErrorMessage = "Problemas ao recuperar o número do celular. Clique em Voltar e informe novamente seu número.")]
        public string UsuarioAcesso_PhoneNumber { get; set; }
    }
}
