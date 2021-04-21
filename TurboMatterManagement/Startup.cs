using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TurboMatterManagement.Startup))]

namespace TurboMatterManagement
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}