using System;
using Android.App;
using SimpleDroid.Db;
using TinyIoC;

namespace SimpleDroid
{
    public class Bootstraper : ITinyIocBootstraper // : TinyIoC.Bootstraper
    {
        private static ITinyIocBootstraper _default;

        public static ITinyIocBootstraper Default => _default ?? (_default = new Bootstraper());

        private Bootstraper()
        {
            _default = this;
        }
        public void ConfigureApplicationContainer(TinyIoCContainer container)
        {                        
            container.Register<IDatabaseFty, DatabaseFty>();

            // Views 
            container.Register<IFragmentFactory, FragmentFactory>().AsSingleton();
            container.Register<Fragment, Views.HomeView>(Resource.Id.nav_home.ToString());
            container.Register<Fragment, Views.MessagesView>(Resource.Id.nav_messages.ToString());

            IDatabase db = null;
            Func<TinyIoCContainer, NamedParameterOverloads, IDatabase> database =
                (c, parameters) =>
                    db ?? (db = new Database(c.Resolve<IDatabaseFty>()));
            container.Register(database);
        }
    }
}