using System;
using UnityEngine;

namespace ProjectOlog.Code.UI.Core
{
    public abstract class BaseScreen : MonoBehaviour
    {
        public abstract Type ModelType { get; }

        public abstract void Show();
        public abstract void Close();
        public abstract void Bind(object model);
        public abstract void Unbind();

        public virtual void Dispose()
        {
        }
    }
}