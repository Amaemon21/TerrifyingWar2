using System;
using Zenject;

namespace MVVM
{
    public abstract class ViewModel : IInitializable, IDisposable
    {
        protected IDisposable Disposable;
        
        public abstract void Initialize();

        public void Dispose()
        {
            Disposable?.Dispose();
        }
    }
}