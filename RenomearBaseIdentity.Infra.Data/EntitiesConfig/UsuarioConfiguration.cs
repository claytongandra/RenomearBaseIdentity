using RenomearBaseIdentity.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace RenomearBaseIdentity.Infra.Data.EntitiesConfig
{
    public class UsuarioConfiguration : EntityTypeConfiguration<Usuario>
    {
        public UsuarioConfiguration()
        {
            HasKey(usu => usu.Usuario_Id);

            Property(usu => usu.Usuario_Id)
                .HasColumnName("usu_id")
                .IsRequired()
                .HasMaxLength(128);

            Property(usu => usu.Usuario_Nome)
                .HasColumnName("usu_nome")
                .IsRequired()
                .HasMaxLength(256);

            Property(usu => usu.Usuario_SobreNome)
                .HasColumnName("usu_sobrenome")
                .IsRequired()
                .HasMaxLength(256);

            Property(usu => usu.Usuario_DataNascimento)
                .HasColumnName("usu_dataNascimento")
               .IsOptional()
               .HasColumnType("datetime2");

            Property(usu => usu.Usuario_Genero)
                .HasColumnName("usu_genero")
               //.IsRequired()
               .HasColumnType("char")
               .HasMaxLength(1);

            Property(usu => usu.Usuario_GeneroDescricao)
                .HasColumnName("usu_generoDescricao")
               .HasMaxLength(256);

            Property(usu => usu.Usuario_FotoPerfil)
                .HasColumnName("usu_fotoPerfil")
                .HasMaxLength(256);

            Property(usu => usu.Usuario_Status)
                .HasColumnName("usu_status")
                .IsRequired()
                .HasColumnType("char")
                .HasMaxLength(1);

            Property(usu => usu.Usuario_DataCadastro)
                .HasColumnName("usu_dataCadastro")
               .IsRequired()
               .HasColumnType("datetime2");

            ToTable("RenomearBaseIdentity_Usuario");
        }
    }
}
