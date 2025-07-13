namespace GeneratorGame.Code.Ecs.Ui
{
    using System;
    using Mono;

    public struct UIViewComponent<T> where T : UIBase
    {
        public UIBase View;
        public Type Type;
    }
}