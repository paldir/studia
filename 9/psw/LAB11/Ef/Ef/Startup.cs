using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Ef.Startup))]
namespace Ef
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
