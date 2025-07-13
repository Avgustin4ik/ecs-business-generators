namespace GeneratorGame.Code.Ecs.Ui.Systems
{
    using Gameplay.Generator;
    using Leopotam.EcsLite;
    using Mono;

    public class OnClickUpgradeSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly GeneratorAspect _aspect;
        private readonly UiAspect _uiAspect;
        private EcsWorld _world;

        public OnClickUpgradeSystem(GeneratorAspect aspect, UiAspect uiAspect)
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
                ref var viewComponent = ref _world.GetPool<UIViewComponent<UIGeneratorModel>>().Get(viewEntity);
                var model = viewComponent.Model;
                if(model.upgrade == default) continue;
                model.upgrade = default;
                ref var link = ref _uiAspect.GeneratorLinked.Get(viewEntity);
                if(!link.Entity.Unpack(_world,out var generatorEntity)) continue;
                ref var generatorComponent = ref _aspect.Generator.Get(generatorEntity);
                
                ref var request = ref _world.GetPool<UpgradeGeneratorRequest>().Add(generatorEntity);
                request.Multiplier = model.upgrade;
            }
        }
    }
}