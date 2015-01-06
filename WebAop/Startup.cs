using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebAop.Startup))]
namespace WebAop
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
