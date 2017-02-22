using System;
using SimpleDroid.Db;
using SimpleDroid.Dialogs;
using TinyIoC;
using Fragment =  Android.Support.V4.App.Fragment;

namespace SimpleDroid
{
    public class Bootstraper : ITinyIocBootstraper
    {        
        public void ConfigureApplicationContainer(TinyIoCContainer container)
        {                        
            container.Register<IDatabaseFty, DatabaseFty>();

            // Views 
            container.Register<IFragmentFactory, FragmentFactory>().AsSingleton();
            container.Register<Fragment, Views.HomeView>(Resource.Id.nav_home.ToString());
            container.Register<Fragment, Views.MessagesView>(Resource.Id.nav_messages.ToString());
            container.Register<Fragment, Views.SettingsView>(Resource.Id.nav_settings.ToString());

            //Dialogs 
            container.Register<IDialog, ExitDialog>(nameof(ExitDialog));

            IDatabase db = null;
            Func<TinyIoCContainer, NamedParameterOverloads, IDatabase> database =
                (c, parameters) =>
                    db ?? (db = new Database(c.Resolve<IDatabaseFty>()));
            container.Register(database);
        }
    }
}