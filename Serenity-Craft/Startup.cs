using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Serenity_Craft.Startup))]
namespace Serenity_Craft
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
