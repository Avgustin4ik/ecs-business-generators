namespace GeneratorGame.Code.Ecs.Ui.Systems
{
    using Components;
    using Gameplay.Generator;
    using Leopotam.EcsLite;
    using Mono;

    public class OnClickUpgradeSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly GeneratorAspect _aspect;
        private readonly UiAspect _uiAspect;
        private EcsWorld _world;
        private EcsFilter _buttonFilter;

        public OnClickUpgradeSystem(GeneratorAspect aspect, UiAspect uiAspect)
        {
            _aspect = aspect;
            _uiAspect = uiAspect;
        }
        
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _buttonFilter = _world.Filter<UIViewComponent<UpgradeButtonModel>>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var button in _buttonFilter)
            {
                ref var viewComponent = ref _uiAspect.UpgradeButton.Get(button);
                var model = viewComponent.Model;
                
                if(!model.OnUpgradeSignal.Take(out var requestGuid)) continue;
                
                foreach (var upgradeEntity in _aspect.AvailableUpgradeFilter)
                {
                    ref var upgradeComponent = ref _aspect.AvailableUpgrade.Get(upgradeEntity);
                    if(upgradeComponent.Guid != requestGuid) continue;
                    _aspect.Purchased.Add(upgradeEntity);
                    foreach (var e in _aspect.BalanceFilter)
                    {
                        ref var balance = ref _aspect.Balance.Get(e);
                        balance.value -= upgradeComponent.Price;
                    }
                    foreach (var genEntity in _aspect.GeneratorFilter)
                    {
                        ref var genComponent = ref _aspect.Generator.Get(genEntity);
                        if(genComponent.Guid != upgradeComponent.GeneratorGuid) continue;
                        ref var upgradeRequest = ref _aspect.Upgrade.Add(genEntity);
                        upgradeRequest.Multiplier = upgradeComponent.Multiplayer;
                        upgradeRequest.generatorGuid = genComponent.Guid;
                    }
                }
            }
        }
    }
}