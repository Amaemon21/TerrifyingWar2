using R3;
using UnityEngine;

namespace MVVM
{
    public abstract class View : MonoBehaviour
    {
        protected CompositeDisposable CompositeDisposable = new();
        
        public virtual void OnDestroy()
        {
            CompositeDisposable?.Dispose();
        }
    }
}