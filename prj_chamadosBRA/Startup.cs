using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(prj_chamadosBRA.Startup))]
namespace prj_chamadosBRA
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
