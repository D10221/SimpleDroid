using System;
using System.Collections.Generic;

namespace SimpleDroid
{
    public static class HaveDisposablesExtensions
    {
        public static void ToBeDisposedBy(this IDisposable disposable, IHaveDisposables owner)
        {
            owner.Disposables.Add(disposable);
        }

        public static void Dispose(this IEnumerable<IDisposable> disposables)
        {            
            foreach (var disposable in disposables)
            {
                disposable?.Dispose();
            }
        }
    }
}