using System;
using R3;
using Zenject;

namespace MVVM
{
    public abstract class ViewModel : IInitializable, IDisposable
    {
        protected CompositeDisposable CompositeDisposable = new();
        
        public abstract void Initialize();

        public void Dispose()
        {
            CompositeDisposable?.Dispose();
        }
    }
}