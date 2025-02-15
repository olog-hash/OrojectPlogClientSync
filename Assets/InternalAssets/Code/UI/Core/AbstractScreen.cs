using System;

namespace ProjectOlog.Code.UI.Core
{
    public abstract class AbstractScreen<TModel> : BaseScreen where TModel:IViewModel
    {
        public override Type ModelType => typeof(TModel);
        protected TModel _model;

        public override void Show()
        {
            gameObject.SetActive(true);
        }

        public override void Close()
        {
            gameObject.SetActive(false);
        }

        public override void Bind(object model)
        {
            if (model is TModel)
                Bind((TModel) model);
        }
        
        public override void Unbind()
        {
            if(_model != null)
                OnUnbind(_model);
        }

        public void Bind(TModel model)
        {
            _model = model;
            OnBind(model);
        }

        public void Unbind(TModel model)
        {
            _model = default(TModel);
            OnUnbind(model);
        }

        protected abstract void OnBind(TModel model);
        protected abstract void OnUnbind(TModel model);
    }
}