namespace GeneratorGame.Code.Ecs.Ui
{
    using System;
    using Mono;

    public struct UIViewComponent<TModel> where TModel : Model, new()
    {
        public UIBase<TModel> View;
        public TModel Model;
        public Type Type;
    }
}