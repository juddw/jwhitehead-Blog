using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(jwhitehead_Blog.Startup))]
namespace jwhitehead_Blog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
