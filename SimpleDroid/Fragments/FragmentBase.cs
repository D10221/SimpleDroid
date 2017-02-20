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
    public class FragmentEventArgs: IEventArgs
    {
        public FragmentEventArgs(string key, object value = null)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; }
        public object Value { get; }
    }

    public abstract class FragmentBase : Fragment, IMenuItemOnMenuItemClickListener, IDisposer, IHaveEvents
    {
        protected abstract int FragmentLayout { get; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            SetHasOptionsMenu(HasOptionsMenu);
            base.OnCreate(savedInstanceState);
        }

        protected abstract bool HasOptionsMenu { get; }
        
        public virtual int ToolbarMenuLayout { get; } = 0;

        private readonly Subject<IEventArgs> _events = new Subject<IEventArgs>();
        private Injector _injector;

        public IObservable<IEventArgs> Events => _events.AsObservable();

        protected void RaiseEvent(object value = null , [CallerMemberName] string key = null)
        {
            _events.OnNext(new FragmentEventArgs(key, value));
        }
        

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            if (ToolbarMenuLayout > 0)
            {
                if (menu != null)
                {
                    menu.Clear();

                    inflater.Inflate(ToolbarMenuLayout, menu);

                    OnMenuInflated(menu);
                }

                for (var i = 0; i < menu.Size(); i++)
                {
                    menu.GetItem(i).SetOnMenuItemClickListener(this);
                }
            }

            base.OnCreateOptionsMenu(menu, inflater);
        }

        public bool OnMenuItemClick(IMenuItem item)
        {
            RaiseEvent(item);
            return true;
        }

        protected virtual void OnMenuInflated(IMenu menu)
        {
            //menu.FindItem(Resource.Id.action_refresh).SetVisible(true);
            //menu.FindItem(Resource.Id.action_attach).SetVisible(false);
        }

        private TinyIoCContainer Container => TinyIoCContainer.Current;
        private Injector Injector => _injector ?? (_injector = new Injector(Container, this));
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            var view = inflater.Inflate(FragmentLayout, container, false);
            // base.OnCreateView(inflater.Inflate(Resource.Layout.homeLayout, container, savedInstanceState)    
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