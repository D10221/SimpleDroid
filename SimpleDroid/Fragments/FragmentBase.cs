using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using Android.OS;
using Android.Views;
using TinyIoC;
using Fragment = Android.Support.V4.App.Fragment;
namespace SimpleDroid
{
    public abstract class FragmentBase : Fragment, ISubscribable, IMenuItemOnMenuItemClickListener, ISubscriber
    {
        private TinyIoCContainer Container => TinyIoCContainer.Current;

        private Injector _injector;
        private Injector Injector => _injector ?? (_injector = new Injector(Container, this));
        protected abstract int FragmentLayout { get; }
        public override void OnCreate(Bundle savedInstanceState)
        {
            HasOptionsMenu = MenuLayout > 0;
            base.OnCreate(savedInstanceState);            
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {            
            var view = inflater.Inflate(FragmentLayout, container, false);            
            Injector.BuildUp();    
            OnViewCreated(view);    
            RaiseEvent(view);
            return view;
        }

        /// <summary>
        /// view doesn't exists on OnCreate
        /// </summary>
        protected virtual void OnViewCreated(View view)
        {
            // ...
        }

        #region Menu

        public virtual int MenuLayout { get; } = 0;
        public virtual IMenu Menu { get; set; }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            menu?.Clear();

            if (MenuLayout > 0 && menu != null)
            {
                inflater.Inflate(MenuLayout, menu);

                OnMenuInflated(menu);

                foreach(var item in menu.Items())
                {
                    item.SetOnMenuItemClickListener(this);
                }
            }

            base.OnCreateOptionsMenu(menu, inflater);
        }

        public virtual bool OnMenuItemClick(IMenuItem item)
        {
            RaiseEvent(item);
            return true;
        }

        protected virtual void OnMenuInflated(IMenu menu)
        {
            Menu = menu;
        }
        

        #endregion

        #region ISubscribable

        private readonly Subject<IEvent> _events = new Subject<IEvent>();

        public IObservable<IEvent> Events => _events.AsObservable();

        protected void RaiseEvent(object value = null , [CallerMemberName] string key = null)
        {
            _events.OnNext(new FragmentEvent(key, value));
        }

        #endregion

        #region ISubscriber

        public CompositeDisposable Subscriptions { get; } = new CompositeDisposable();

        public override void OnDestroy()
        {
            Subscriptions.Dispose();
            base.OnDestroy();
        }

        #endregion
    }
}