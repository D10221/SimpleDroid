using System;
using System.Reflection;
using Android.Content;
using NLog;
using TinyIoC;

namespace SimpleDroid
{
    public class Injector
    {
        readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private TinyIoCContainer Container { get; }
        readonly object _target;
        public Injector(TinyIoCContainer container, object target)
        {
            Container = container;
            _target = target;
        }

        private bool _isBuilt;
        public void BuildUp()
        {
            if (_isBuilt) return;
            _isBuilt = true;

            try
            {
                foreach (var property in PropertyInfos)
                    BuildUp(property.GetCustomAttribute<InjectAttribute>(), property);
            }
            catch (Exception e)
            {
                _logger.Error(e);
                throw;
            }
        }


        private PropertyInfo[] _propertyInfos;

        private PropertyInfo[] PropertyInfos
            =>
                _propertyInfos ??
                (_propertyInfos =
                    _target.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance));

        private void BuildUp(InjectAttribute inject, PropertyInfo property)
        {
            if (inject == null) return;
            try
            {
                var value = Resolve(
                    inject.Type ?? property.PropertyType,
                    inject.Name ?? (inject.IsNamed ? property.Name : null),
                    inject.Optional
                );
                if (value != null)
                {
                    property.SetValue(_target, value);
                }
            }
            catch (TinyIoCResolutionException)
            {
                if (inject.Optional) return;
                throw;
            }
            catch (Exception e)
            {
                _logger.Error(e);
                throw;
            }           
        }

        private object OnBuildUp(object resolve)
        {             
            if (_target is ContextWrapper)
            {
                (resolve as ViewModelBase)?.BuildUp((ContextWrapper) _target);
            }
            return resolve;
        }

        private object Resolve(Type type, string name = null,bool optional = false )
        {
            return name == null ? Container.Resolve(type) : Container.Resolve(type, name);
        }
    }
}