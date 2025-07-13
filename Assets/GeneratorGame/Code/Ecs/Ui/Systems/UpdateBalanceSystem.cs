namespace GeneratorGame.Code.Ecs.Ui.Systems
{
    using Leopotam.EcsLite;
    using Mono;
    using UnityEngine;

    public class UpdateBalanceSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _viewFilter;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _viewFilter = _world.Filter<UIViewComponent<UIGeneratorView>>().End();
        }

        public void Run(IEcsSystems systems)
        {
        }
    }
}