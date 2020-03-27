using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GraceCareCenterOrder.Startup))]
namespace GraceCareCenterOrder
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
