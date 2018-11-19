using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Ingredients2.Startup))]
namespace Ingredients2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
