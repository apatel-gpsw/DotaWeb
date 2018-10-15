using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DotaWeb.Startup))]
namespace DotaWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
