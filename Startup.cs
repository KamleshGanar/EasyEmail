using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EmailTest.Startup))]
namespace EmailTest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
