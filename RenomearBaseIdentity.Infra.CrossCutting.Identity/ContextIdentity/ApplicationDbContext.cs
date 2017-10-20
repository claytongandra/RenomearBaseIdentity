using Microsoft.AspNet.Identity.EntityFramework;
using RenomearBaseIdentity.Infra.CrossCutting.Identity.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using RenomearBaseIdentity.Infra.Data.EntitiesConfig;

namespace RenomearBaseIdentity.Infra.CrossCutting.Identity.ContextIdentity
{
    // public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IDisposable
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>, IDisposable
    {
        public ApplicationDbContext()
            : base("RenomearBaseIdentityConnection")
        //: base("RenomearBaseIdentityConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            //modelBuilder.Properties()
            //  .Where(p => p.Name.Contains("_Id"))
            //  .Configure(p => p.IsKey());

            //modelBuilder.Properties<string>()
            //  .Configure(p => p.HasColumnType("varchar"));

            //modelBuilder.Properties<string>()
            //  .Configure(p => p.HasMaxLength(128));

           


            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new UsuarioConfiguration());

            //  modelBuilder.HasDefaultSchema("");

            modelBuilder.Entity<ApplicationUser>().ToTable("RenomearBaseIdentity_UsuarioAcesso").Property(p => p.Id).HasColumnName("uac_id");
            modelBuilder.Entity<ApplicationUser>().ToTable("RenomearBaseIdentity_UsuarioAcesso").Property(p => p.UserName).HasColumnName("uac_userName");
            modelBuilder.Entity<ApplicationUser>().ToTable("RenomearBaseIdentity_UsuarioAcesso").Property(p => p.Email).HasColumnName("uac_email").IsRequired();
            modelBuilder.Entity<ApplicationUser>().ToTable("RenomearBaseIdentity_UsuarioAcesso").Property(p => p.EmailConfirmed).HasColumnName("uac_emailConfirmed");
            modelBuilder.Entity<ApplicationUser>().ToTable("RenomearBaseIdentity_UsuarioAcesso").Property(p => p.PasswordHash).HasColumnName("uac_passwordHash");
            modelBuilder.Entity<ApplicationUser>().ToTable("RenomearBaseIdentity_UsuarioAcesso").Property(p => p.SecurityStamp).HasColumnName("uac_securityStamp");
            modelBuilder.Entity<ApplicationUser>().ToTable("RenomearBaseIdentity_UsuarioAcesso").Property(p => p.PhoneNumber).HasColumnName("uac_phoneNumber");
            modelBuilder.Entity<ApplicationUser>().ToTable("RenomearBaseIdentity_UsuarioAcesso").Property(p => p.PhoneNumberConfirmed).HasColumnName("uac_phoneNumberConfirmed");
            modelBuilder.Entity<ApplicationUser>().ToTable("RenomearBaseIdentity_UsuarioAcesso").Property(p => p.TwoFactorEnabled).HasColumnName("uac_twoFactorEnabled");
            modelBuilder.Entity<ApplicationUser>().ToTable("RenomearBaseIdentity_UsuarioAcesso").Property(p => p.LockoutEndDateUtc).HasColumnName("uac_lockoutEndDateUtc");
            modelBuilder.Entity<ApplicationUser>().ToTable("RenomearBaseIdentity_UsuarioAcesso").Property(p => p.LockoutEnabled).HasColumnName("uac_lockoutEnabled");
            modelBuilder.Entity<ApplicationUser>().ToTable("RenomearBaseIdentity_UsuarioAcesso").Property(p => p.AccessFailedCount).HasColumnName("uac_accessFailedCount");
            modelBuilder.Entity<ApplicationUser>().ToTable("RenomearBaseIdentity_UsuarioAcesso").Property(p => p.UsuarioAcesso_Nivel).HasColumnName("uac_nivelAcesso"); //.IsOptional();
            modelBuilder.Entity<ApplicationUser>().ToTable("RenomearBaseIdentity_UsuarioAcesso").HasRequired(x => x.UsuarioAcesso_Usuario).WithRequiredDependent().Map(p => p.MapKey("uac_fk_usuario"));

            modelBuilder.Entity<IdentityUserRole>().ToTable("RenomearBaseIdentity_UsuariosRoles").Property(p => p.UserId).HasColumnName("uro_usuarioId");
            modelBuilder.Entity<IdentityUserRole>().ToTable("RenomearBaseIdentity_UsuariosRoles").Property(p => p.RoleId).HasColumnName("uro_roleId");

            modelBuilder.Entity<IdentityUserLogin>().ToTable("RenomearBaseIdentity_UsuariosLogins").Property(p => p.UserId).HasColumnName("ulo_usuarioId");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("RenomearBaseIdentity_UsuariosLogins").Property(p => p.LoginProvider).HasColumnName("ulo_loginProvider");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("RenomearBaseIdentity_UsuariosLogins").Property(p => p.ProviderKey).HasColumnName("ulo_providerKey");

            modelBuilder.Entity<IdentityUserClaim>().ToTable("RenomearBaseIdentity_UsuariosClaims").Property(p => p.Id).HasColumnName("ucl_id");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("RenomearBaseIdentity_UsuariosClaims").Property(p => p.UserId).HasColumnName("ucl_usuarioId");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("RenomearBaseIdentity_UsuariosClaims").Property(p => p.ClaimType).HasColumnName("ucl_claimType");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("RenomearBaseIdentity_UsuariosClaims").Property(p => p.ClaimValue).HasColumnName("ucl_claimValue");

            modelBuilder.Entity<IdentityRole>().ToTable("RenomearBaseIdentity_Roles").Property(p => p.Id).HasColumnName("rol_id");
            modelBuilder.Entity<IdentityRole>().ToTable("RenomearBaseIdentity_Roles").Property(p => p.Name).HasColumnName("rol_name");


            //////// Mapping for ApiRole
            //////modelBuilder.Entity<IdentityRole>().Map(c =>
            //////{
            //////    c.ToTable("RenomearBaseIdentity_Roles");
            //////    c.Property(p => p.Id).HasColumnName("rol_id");
            //////    c.Properties(p => new
            //////    {
            //////        p.Name
            //////    });
            //////    c.Property(p => p.Name).HasColumnName("rol_name");
            //////}).HasKey(p => p.Id);
            //////modelBuilder.Entity<IdentityRole>().HasMany(c => c.Users).WithRequired().HasForeignKey(c => c.RoleId);


            //////modelBuilder.Entity<ApplicationUser>().Map(c =>
            //////{
            //////    c.ToTable("RenomearBaseIdentity_UsuarioAcesso");
            //////    c.Property(p => p.Id).HasColumnName("uac_id");
            //////    c.Properties(p => new
            //////    {
            //////        p.AccessFailedCount,
            //////        p.Email,
            //////        p.EmailConfirmed,
            //////        p.PasswordHash,
            //////        p.PhoneNumber,
            //////        p.PhoneNumberConfirmed,
            //////        p.TwoFactorEnabled,
            //////        p.SecurityStamp,
            //////        p.LockoutEnabled,
            //////        p.LockoutEndDateUtc,
            //////        p.UserName
            //////    });
            //////}).HasKey(c => c.Id);
            //////modelBuilder.Entity<ApplicationUser>().HasMany(c => c.Logins).WithOptional().HasForeignKey(c => c.UserId);
            //////modelBuilder.Entity<ApplicationUser>().HasMany(c => c.Claims).WithOptional().HasForeignKey(c => c.UserId);
            //////modelBuilder.Entity<ApplicationUser>().HasMany(c => c.Roles).WithRequired().HasForeignKey(c => c.UserId);

        }
    }
}
