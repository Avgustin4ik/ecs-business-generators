namespace GeneratorGame.Code.Ecs.Ui
{
    using System;
    using Leopotam.EcsLite;

    public class UiFeature : EcsSystems
    {
        public UiFeature(EcsWorld world, object shared = null) : base(world, shared)
        {
        }
    }
}