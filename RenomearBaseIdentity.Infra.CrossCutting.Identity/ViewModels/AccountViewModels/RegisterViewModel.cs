using System;
using System.ComponentModel.DataAnnotations;

namespace RenomearBaseIdentity.Infra.CrossCutting.Identity.ViewModels.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Usuário")]
        public string UsuarioAcesso_UserName { get; set; }

        [Required]
        [Display(Name = "Nome")]
        public string Usuario_Nome { get; set; }

        [Required]
        [Display(Name = "Sobrenome")]
        public string Usuario_SobreNome { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string UsuarioAcesso_Email { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Data Nascimento")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Usuario_DataNascimento { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "Selecione o genero.")]
        [Display(Name = "Genero")]
        public string Usuario_Genero { get; set; }

        public string Usuario_GeneroDescricao { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "A {0} deve conter pelo menos {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Preencha o campo Confirmação da Senha.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmação da Senha")]
        [Compare("Password", ErrorMessage = "A senha e a confirmação da senha estão diferentes.")]
        public string ConfirmPassword { get; set; }

    }
}
