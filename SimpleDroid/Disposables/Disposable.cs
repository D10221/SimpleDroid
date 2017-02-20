using System;
using System.Collections.Generic;

namespace SimpleDroid
{
    public interface IDisposer
    {
        IList<IDisposable> Disposables { get; }
    }
    public class Disposable : IDisposable
    {
        public bool Disposed { get; private set; }
        public string Name { get; }

        private readonly Action _dispose;
        
        public Disposable(Action dispose, string name = null)
        {
            _dispose = dispose;
            Name = name;
        }

        public void Dispose()
        {
            if (Disposed) return;
            Disposed = true;
            _dispose();
        }
    }
}