using System;

namespace SimpleDroid
{
    class Disposable: IDisposable
    {        
        private readonly Action _dispose;

        public Disposable(Action dispose)
        {
            _dispose = dispose;
        }

        public void Dispose()
        {
            _dispose?.Invoke();
        }
    }
}