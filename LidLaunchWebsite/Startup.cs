using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LidLaunchWebsite.Startup))]
namespace LidLaunchWebsite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {

        }
    }
}
