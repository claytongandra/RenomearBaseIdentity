using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SimpleInjector;
using RenomearBaseIdentity.Application.Services;
using RenomearBaseIdentity.Application.Interfaces;
using RenomearBaseIdentity.Domain.Interfaces.Services;
using RenomearBaseIdentity.Domain.Services;
using RenomearBaseIdentity.Infra.Data.Repositories;
using RenomearBaseIdentity.Infra.CrossCutting.Identity.Configuration;
using RenomearBaseIdentity.Infra.CrossCutting.Identity.ContextIdentity;
using RenomearBaseIdentity.Infra.CrossCutting.Identity.ViewModels;
using RenomearBaseIdentity.Domain.Interfaces.Repositories;
using RenomearBaseIdentity.Infra.Data;

namespace RenomearBaseIdentity.Infra.CrossCutting.IoC
{
    public class BootStrapper
    {
        public static void RegisterServices(Container container)
        {
            container.Register<ApplicationDbContext>(Lifestyle.Scoped);

          //  container.Register<IUserStore<ApplicationUser>>(() => new UserStore<ApplicationUser>(new ApplicationDbContext()), Lifestyle.Scoped);

            container.Register<IUserStore<ApplicationUser,string>>(() => new UserStore<ApplicationUser, ApplicationRole, string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>(new ApplicationDbContext()), Lifestyle.Scoped);

            // container.Register<IUserStore<ApplicationUser>>(() => new UserStore<ApplicationUser>(),Lifestyle.Scoped);

            // container.Register<IUserStore<ApplicationUser>>(() => new UserStore<ApplicationUser>(new ApplicationDbContext()), Lifestyle.Scoped);

            //container.Register<UserManager<ApplicationUser, string>, ApplicationUserManager>();


            //container.Register(() => new ApplicationUserManager(new UserStore<ApplicationUser>()));


            container.Register(() => new ApplicationUserStore(new ApplicationDbContext()), Lifestyle.Scoped);
            container.RegisterCollection(typeof(IUserStore<ApplicationUser, string>), new[] { typeof(ApplicationUserStore) });


            container.Register<IRoleStore<IdentityRole, string>>(() => new RoleStore<IdentityRole>(), Lifestyle.Scoped);
            container.Register<ApplicationRoleManager>(Lifestyle.Scoped);
            container.Register<ApplicationUserManager>(Lifestyle.Scoped);
            container.Register<ApplicationSignInManager>(Lifestyle.Scoped);

            container.Register<UsuarioContext>(Lifestyle.Scoped);

            container.Register<IUsuarioAcessoRepository, UsuarioAcessoRepository>(Lifestyle.Scoped);
            container.Register<IUsuarioAcessoAppService, UsuarioAcessoAppService>();
            container.Register<IUsuarioAcessoService, UsuarioAcessoService>();
        }
    }
}
