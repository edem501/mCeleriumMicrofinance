using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(iCelerium.Startup))]
namespace iCelerium
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
