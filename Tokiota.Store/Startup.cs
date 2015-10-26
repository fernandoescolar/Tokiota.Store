[assembly: Microsoft.Owin.OwinStartupAttribute(typeof(Tokiota.Store.Startup))]
namespace Tokiota.Store
{
    using Owin;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
