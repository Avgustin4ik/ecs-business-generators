namespace GeneratorGame.Code.Ecs.Ui.Systems
{
    using Gameplay.Generator;
    using Leopotam.EcsLite;
    using Mono;

    public class UpdateUpgradeButtonViewSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private readonly UiAspect _uiAspect;
        private readonly GeneratorAspect _generatorAspect;
        private EcsFilter _viewFilter;

        public UpdateUpgradeButtonViewSystem(GeneratorAspect generatorAspect, UiAspect uiAspect)
        {
            _generatorAspect = generatorAspect;
            _uiAspect = uiAspect;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _viewFilter = _world.Filter<UIViewComponent<UpgradeButtonModel>>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var viewEntity in _viewFilter)
            {
                ref var viewComponent = ref _uiAspect.UpgradeButton.Get(viewEntity);
                var model = viewComponent.Model;
                foreach (var entity in _generatorAspect.AvailableUpgradeFilter)
                {
                    ref var component = ref _generatorAspect.AvailableUpgrade.Get(entity);
                    if (model.Guid != component.Guid) continue;

                    model.Multiplayer.Value = component.Multiplayer;
                    model.Name.Value = component.Name;
                    model.Price.Value = component.Price;
                    var isPurchased = _generatorAspect.Purchased.Has(entity);
                    model.IsPurchased.Value = isPurchased;
                    if (isPurchased) continue;

                    foreach (var e in _generatorAspect.GeneratorFilter)
                    {
                        ref var generator = ref _generatorAspect.Generator.Get(e);
                        
                        if(generator.Guid!= component.GeneratorGuid) continue;
                        if (generator.Level > 0)
                        {
                            foreach (var balanceEntity in _generatorAspect.BalanceFilter)
                            {
                                ref var balance = ref _generatorAspect.Balance.Get(balanceEntity);
                                model.IsInteractable.Value = balance.value >= component.Price;
                            }
                        }
                        else
                        {
                            model.IsInteractable.Value = false;
                        }
                    }
                }
            }
        }
    }
}