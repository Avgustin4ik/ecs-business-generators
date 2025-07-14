namespace GeneratorGame.Code.Ecs.Ui.Systems
{
    using Gameplay.Generator;
    using Leopotam.EcsLite;

    public class UpdateProgressSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly GeneratorAspect _aspect;
        private readonly UiAspect _uiAspect;
        private EcsWorld _world;

        public UpdateProgressSystem(GeneratorAspect aspect, UiAspect uiAspect)
        {
            _aspect = aspect;
            _uiAspect = uiAspect;
        }
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var viewEntity in _uiAspect.GeneratorViewFilter)
            {
                ref var viewComponent = ref _uiAspect.GeneratorView.Get(viewEntity);
                ref var link = ref _uiAspect.GeneratorLinked.Get(viewEntity);
                if(!link.Entity.Unpack(_world,out var generatorEntity)) continue;
                
                ref var generatorComponent = ref _aspect.Generator.Get(generatorEntity);
                var model = viewComponent.Model;
                model.progress.Value = generatorComponent.Progress / generatorComponent.DurationInSeconds;
            }
        }
    }
}