using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OnlineContestSystem.Startup))]
namespace OnlineContestSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
