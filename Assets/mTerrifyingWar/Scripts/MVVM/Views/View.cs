using System;
using UnityEngine;

namespace MVVM
{
    public abstract class View : MonoBehaviour
    {
        protected IDisposable Disposable;
        
        public void OnDisable()
        {
            Disposable?.Dispose();
        }
    }
}