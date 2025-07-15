namespace GeneratorGame.Code.Ecs.Ui.Systems
{
    using Gameplay.Generator;
    using Leopotam.EcsLite;

    public class UpdateGeneratorViewSystem : IEcsRunSystem, IEcsInitSystem
    {
        private readonly GeneratorAspect _generatorAspect;
        private readonly UiAspect _uiAspect;
        private EcsWorld _world;

        public UpdateGeneratorViewSystem(GeneratorAspect generatorAspect,UiAspect uiAspect)
        {
            _generatorAspect = generatorAspect;
            _uiAspect = uiAspect;
        }
        
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var view in _uiAspect.GeneratorViewFilter)
            {
                ref var linkedComponent = ref _uiAspect.GeneratorLinked.Get(view);
                if(!linkedComponent.Entity.Unpack(_world,out var generatorEntity)) continue;
                ref var generator = ref _generatorAspect.Generator.Get(generatorEntity);

                ref var viewComponent = ref _uiAspect.GeneratorView.Get(view);
                var model = viewComponent.Model;
                model.income.Value = generator.CurrentIncome;
                model.label.Value = generator.Name;
            }
        }
    }
}