using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(orm.Startup))]
namespace orm
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
