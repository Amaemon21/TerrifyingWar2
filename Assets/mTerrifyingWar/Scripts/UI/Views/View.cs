using System;
using R3;
using UnityEngine;

namespace MVVM
{
    public abstract class View : MonoBehaviour
    {
        protected IDisposable Disposable;
        protected CompositeDisposable CompositeDisposable = new();
        
        public void OnDestroy()
        {
            Disposable?.Dispose();
            CompositeDisposable?.Dispose();
        }
    }
}