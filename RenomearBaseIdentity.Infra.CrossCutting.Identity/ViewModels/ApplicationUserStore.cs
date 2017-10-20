using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RenomearBaseIdentity.Infra.CrossCutting.Identity.ContextIdentity;



namespace RenomearBaseIdentity.Infra.CrossCutting.Identity.ViewModels
{
    public class ApplicationUserStore :
    UserStore<ApplicationUser, ApplicationRole, string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>,
    IUserStore<ApplicationUser>,
    IDisposable
    {
        public ApplicationUserStore(ApplicationDbContext context) : base(context) { }
    }
}
