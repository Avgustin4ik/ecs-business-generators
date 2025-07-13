namespace GeneratorGame.Code.Ecs.Ui.Systems
{
    using Gameplay.Generator;
    using Leopotam.EcsLite;
    using Mono;

    public class UpdateProgressSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _viewFilter;
        private EcsFilter _generatorFilter;
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _viewFilter = _world.Filter<UIViewComponent<UIGeneratorView>>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var viewEntity in _viewFilter)
            {
                
            }
        }
    }
}