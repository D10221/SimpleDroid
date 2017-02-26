using System.Runtime.CompilerServices;
using TinyIoC;

namespace SimpleDroid.Extensionsss
{
    public static class Extensionsss
    {
        /// <summary>
        ///  Convetion , calle member name is the named instace/derivative
        /// </summary>
        public static T ResolveMe<T>(this TinyIoCContainer container,[CallerMemberName]string name = null) where T : class
        {
            return container.Resolve<T>(name);
        }

    }
}