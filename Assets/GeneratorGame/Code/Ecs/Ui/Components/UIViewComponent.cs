namespace GeneratorGame.Code.Ecs.Ui.Components
{
    using System;
    using GeneratorGame.Code.Ecs.Ui.Mono;

    public struct UIViewComponent<TModel> where TModel : Model, new()
    {
        public UIBase<TModel> View;
        public TModel Model;
        public Type Type;
    }
}