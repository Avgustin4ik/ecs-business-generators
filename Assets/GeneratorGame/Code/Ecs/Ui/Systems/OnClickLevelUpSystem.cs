namespace GeneratorGame.Code.Ecs.Ui.Systems
{
    using Gameplay.Generator;
    using Leopotam.EcsLite;
    using Mono;

    public class OnClickLevelUpSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly GeneratorAspect _aspect;
        private readonly UiAspect _uiAspect;
        private EcsWorld _world;

        public OnClickLevelUpSystem(GeneratorAspect aspect, UiAspect uiAspect)
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
                var model = viewComponent.Model;
                
                if(!model.OnLevelUpSignal.Take()) continue;
                
                ref var link = ref _uiAspect.GeneratorLinked.Get(viewEntity);
                if(!link.Entity.Unpack(_world,out var generatorEntity)) continue;
                ref var request = ref _world.GetPool<LevelUpGeneratorRequest>().Add(generatorEntity);
                ref var levelUpPrice = ref _aspect.LevelUpPrice.Get(generatorEntity);
                foreach (var entity in _aspect.BalanceFilter)
                {
                    ref var balanceComponent = ref _aspect.Balance.Get(entity);
                    balanceComponent.value -= levelUpPrice.Next;
                }
            }
        }
    }
}