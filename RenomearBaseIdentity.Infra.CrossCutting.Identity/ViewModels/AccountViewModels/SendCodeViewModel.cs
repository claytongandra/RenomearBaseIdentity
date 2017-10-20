using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace RenomearBaseIdentity.Infra.CrossCutting.Identity.ViewModels.AccountViewModels
{
    public class SendCodeViewModel
    {
        [Required(ErrorMessage = "Informe Provedor de dois fatores de Autenticação")]
        [Display(Name = "Provedor de dois fatores de Autenticação")]
        public string SelectedProvider { get; set; }

        public ICollection<SelectListItem> Providers { get; set; }

        [ScaffoldColumn(false)]
        public string ReturnUrl { get; set; }

        [ScaffoldColumn(false)]
        public bool RememberMe { get; set; }
    }
}
