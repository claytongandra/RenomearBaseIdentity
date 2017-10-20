using Microsoft.Owin;
using Owin;
using RenomearBaseIdentity.UI.Site;

[assembly: OwinStartup(typeof(Startup))]

namespace RenomearBaseIdentity.UI.Site
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
