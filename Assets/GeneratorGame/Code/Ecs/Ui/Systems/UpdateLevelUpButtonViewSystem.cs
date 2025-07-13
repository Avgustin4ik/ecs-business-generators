namespace GeneratorGame.Code.Ecs.Ui.Systems
{
    using Gameplay.Generator;
    using Leopotam.EcsLite;

    public class UpdateLevelUpButtonViewSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private readonly UiAspect _uiAspect;
        private readonly GeneratorAspect _generatorAspect;

        public UpdateLevelUpButtonViewSystem(GeneratorAspect generatorAspect, UiAspect uiAspect)
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
            foreach (var viewEntity in _uiAspect.GeneratorViewFilter)
            {
                ref var viewComponent = ref _uiAspect.GeneratorView.Get(viewEntity);
                var model = viewComponent.Model;
                
                ref var linkedComponent = ref _uiAspect.GeneratorLinked.Get(viewEntity);
                if (!linkedComponent.Entity.Unpack(_world, out var generatorEntity)) continue;
                
                ref var generator = ref _generatorAspect.Generator.Get(generatorEntity);
                ref var levelUpPriceComponent = ref _generatorAspect.LevelUpPrice.Get(generatorEntity);
                
                model.levelupPrice.Value = levelUpPriceComponent.Next;
                model.level.Value = generator.Level;
                foreach (var balanceEntity in _generatorAspect.BalanceFilter)
                {
                    ref var balance = ref _generatorAspect.Balance.Get(balanceEntity);
                    model.levelUpAvailable.Value = balance.value >= levelUpPriceComponent.Next;
                }
            }
        }
    }
}