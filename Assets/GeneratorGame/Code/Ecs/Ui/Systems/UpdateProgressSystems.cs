namespace GeneratorGame.Code.Ecs.Ui.Systems
{
    using System;
    using Components;
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
            _viewFilter = _world.Filter<UIViewComponent<UIGeneratorModel>>().Inc<GeneratorGuid>().End();
            _generatorFilter = _world.Filter<GeneratorComponent>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var viewEntity in _viewFilter)
            {
                ref var viewComponent = ref _world.GetPool<UIViewComponent<UIGeneratorModel>>().Get(viewEntity);
                foreach (var generator in _generatorFilter)
                {
                    ref var generatorComponent = ref _world.GetPool<GeneratorComponent>().Get(generator);
                    ref var generatorGuid = ref _world.GetPool<GeneratorGuid>().Get(viewEntity);
                    if (generatorComponent.Guid != generatorGuid.Guid) continue;

                    var model = viewComponent.Model;
                    model.progress.Value = generatorComponent.Progress / generatorComponent.DurationInSeconds;
                }
            }
        }
    }
}