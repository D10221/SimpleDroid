using System;
using System.Collections.Generic;

namespace SimpleDroid
{
    public interface IHaveDisposables: IDisposable
    {
        IList<IDisposable> Disposables { get; }
    }
}