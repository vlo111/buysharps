using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(buysharps.Startup))]
namespace buysharps
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
