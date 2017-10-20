using RenomearBaseIdentity.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace RenomearBaseIdentity.Infra.Data.EntitiesConfig
{
    public class UsuarioAcessoConfiguration : EntityTypeConfiguration<UsuarioAcesso>
    {
        public UsuarioAcessoConfiguration()
        {
            HasKey(uac => uac.UsuarioAcesso_Id);

            Property(uac => uac.UsuarioAcesso_Id)
                .HasColumnName("uac_id");

            Property(uac => uac.UsuarioAcesso_UserName)
                .HasColumnName("uac_userName");

            Property(uac => uac.UsuarioAcesso_Email)
                .HasColumnName("uac_email");

            Property(uac => uac.UsuarioAcesso_EmailConfirmed)
                .HasColumnName("uac_emailConfirmed");

            Property(uac => uac.UsuarioAcesso_PasswordHash)
                .HasColumnName("uac_passwordHash");

            Property(uac => uac.UsuarioAcesso_SecurityStamp)
                .HasColumnName("uac_securityStamp");

            Property(uac => uac.UsuarioAcesso_PhoneNumber)
                .HasColumnName("uac_phoneNumber");

            Property(uac => uac.UsuarioAcesso_PhoneNumberConfirmed)
                .HasColumnName("uac_phoneNumberConfirmed");

            Property(uac => uac.UsuarioAcesso_TwoFactorEnabled)
                .HasColumnName("uac_twoFactorEnabled");

            Property(uac => uac.UsuarioAcesso_LockoutEndDateUtc)
                .HasColumnName("uac_lockoutEndDateUtc")
                .HasColumnType("datetime2");

            Property(uac => uac.UsuarioAcesso_LockoutEnabled)
                .HasColumnName("uac_lockoutEnabled");

            Property(uac => uac.UsuarioAcesso_AccessFailedCount)
                .HasColumnName("uac_accessFailedCount");

            Property(uac => uac.UsuarioAcesso_Nivel)
                .HasColumnName("uac_nivelAcesso");

            Property(uac => uac.UsuarioAcesso_CodigoUsuario)
               .HasColumnName("uac_fk_usuario");

            HasRequired(uac => uac.UsuarioAcesso_Usuario)
                .WithMany()
                .HasForeignKey(uac => uac.UsuarioAcesso_CodigoUsuario);

            ToTable("RenomearBaseIdentity_UsuarioAcesso");
        }
    }
}
