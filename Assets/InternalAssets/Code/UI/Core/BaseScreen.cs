using System;
using R3;
using UnityEngine;

namespace ProjectOlog.Code.UI.Core
{
    public abstract class BaseScreen : MonoBehaviour
    {
        public abstract Type ModelType { get; }
        protected CompositeDisposable _disposables = new CompositeDisposable();

        public abstract void Show();
        public abstract void Hide();
        public abstract void Bind(object model);
        public abstract void Unbind();

        protected virtual void OnDestroy() => _disposables.Dispose();
        public virtual void Dispose() => _disposables.Dispose();
    }
}