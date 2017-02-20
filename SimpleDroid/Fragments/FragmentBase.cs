using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using Android.App;
using Android.OS;
using Android.Views;
using TinyIoC;

namespace SimpleDroid
{
    public abstract class FragmentBase : Fragment, IView, IMenuItemOnMenuItemClickListener, IDisposer
    {
        protected abstract int FragmentLayout { get; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            SetHasOptionsMenu(true);
            base.OnCreate(savedInstanceState);
        }
        
        private readonly Subject<IEventArgs> _events = new Subject<IEventArgs>();
        
        public IObservable<IEventArgs> Events => _events.AsObservable();

        protected void RaiseEvent(object value = null , [CallerMemberName] string key = null)
        {
            _events.OnNext(new FragmentEventArgs(key, value));
        }
        public virtual int ToolbarMenuLayout { get; } = 0;

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            menu?.Clear();

            if (ToolbarMenuLayout > 0 && menu != null)
            {
                inflater.Inflate(ToolbarMenuLayout, menu);

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

        public virtual IMenu Menu { get; set; }

        protected virtual void OnMenuInflated(IMenu menu)
        {
            Menu = menu;
        }

        private TinyIoCContainer Container => TinyIoCContainer.Current;

        private Injector _injector;
        private Injector Injector => _injector ?? (_injector = new Injector(Container, this));
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {            
            var view = inflater.Inflate(FragmentLayout, container, false);            
            Injector.BuildUp();        
            RaiseEvent(view);
            return view;
        }

        public IList<IDisposable> Disposables { get; } = new List<IDisposable>();
        public override void OnDestroy()
        {
            Disposables.Dispose();
            base.OnDestroy();
        }

        
    }
}