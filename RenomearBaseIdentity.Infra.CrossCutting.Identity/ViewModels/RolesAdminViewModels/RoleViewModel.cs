using System.ComponentModel.DataAnnotations;

namespace RenomearBaseIdentity.Infra.CrossCutting.Identity.ViewModels.RolesAdminViewModels
{
    public class RoleViewModel
    {
        [ScaffoldColumn(false)]
        // [Required(ErrorMessage = "Problemas ao recuperar o número do celular. Clique em Voltar e informe novamente seu número.")]
        public string Role_Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Nome da Role")]
        public string Role_Nome { get; set; }
    }
}
