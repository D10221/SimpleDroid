using System;
using System.Collections.Generic;

namespace SimpleDroid
{
    public static class DisposableExtensions
    {
        public static void ToBeDisposedBy(this IDisposable disposable , IDisposer disposer)
        {
            disposer.Disposables.Add(disposable);
        }
        public static void ToBeDisposedBy(this IDisposable disposable , IDisposer disposer, string name)
        {
            disposer.Disposables.Add(new Disposable(disposable.Dispose, name));
        }

        public static void Dispose(this IEnumerable<IDisposable> disposables)
        {
            foreach (var disposable in disposables)
            {
                disposable?.Dispose();
            }
        }

        public static void Dispose(this IDisposer dispopser)
        {
            dispopser?.Disposables?.Dispose();
        }
    }
}