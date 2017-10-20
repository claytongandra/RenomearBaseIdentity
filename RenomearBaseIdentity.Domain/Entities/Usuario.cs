using System;

namespace RenomearBaseIdentity.Domain.Entities
{
    public class Usuario
    {
        public Usuario()
        {
            Usuario_Id = Guid.NewGuid().ToString();
        }

        public string Usuario_Id { get; set; }
        public string Usuario_Nome { get; set; }
        public string Usuario_SobreNome { get; set; }
        public DateTime? Usuario_DataNascimento { get; set; }
        public string Usuario_Genero { get; set; }
        public string Usuario_GeneroDescricao { get; set; }
        public string Usuario_FotoPerfil { get; set; }
        public string Usuario_Status { get; set; }
        public DateTime? Usuario_DataCadastro { get; set; }
    }
}
