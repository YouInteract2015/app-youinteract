using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(portalYI.Startup))]
namespace portalYI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
