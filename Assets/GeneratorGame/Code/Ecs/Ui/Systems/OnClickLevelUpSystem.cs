namespace GeneratorGame.Code.Ecs.Ui.Systems
{
    using Components;
    using Gameplay.Generator;
    using Leopotam.EcsLite;
    using Mono;

    public class OnClickLevelUpSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _viewFilter;
        private EcsFilter _generatorFilter;
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _viewFilter = _world.Filter<UIViewComponent<UIGeneratorModel>>().Inc<GeneratorGuid>().End();
            _generatorFilter = _world.Filter<GeneratorComponent>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var viewEntity in _viewFilter)
            {
                ref var viewComponent = ref _world.GetPool<UIViewComponent<UIGeneratorModel>>().Get(viewEntity);
                var model = viewComponent.Model;
                if(!model.levelup) continue;
                model.levelup = false;
                ref var generatorGuid = ref _world.GetPool<GeneratorGuid>().Get(viewEntity);
                
                foreach (var generatorEntity in _generatorFilter)
                {
                    ref var generatorComponent = ref _world.GetPool<GeneratorComponent>().Get(generatorEntity);
                    if (generatorComponent.Guid != generatorGuid.Guid) continue;

                    ref var request = ref _world.GetPool<LevelUpGeneratorRequest>().Add(generatorEntity);
                    request.generatorGuid = generatorGuid.Guid;

                }
            }
        }
    }
}