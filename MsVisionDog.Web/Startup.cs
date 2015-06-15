using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MsVisionDog.Web.Startup))]
namespace MsVisionDog.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
