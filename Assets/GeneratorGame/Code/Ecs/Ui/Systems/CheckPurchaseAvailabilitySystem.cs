namespace GeneratorGame.Code.Ecs.Ui.Systems
{
    using Gameplay.Player;
    using Leopotam.EcsLite;
    using Mono;

    public class CheckPurchaseAvailabilitySystem : IEcsInitSystem,IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _buttonFilter;
        private EcsFilter _balanceFilter;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _buttonFilter = _world.Filter<UIViewComponent<PurchaseButtonModel>>().End();
            _balanceFilter = _world.Filter<BalanceComponent>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _buttonFilter)
            {
                ref var viewComponent = ref _world.GetPool<UIViewComponent<PurchaseButtonModel>>().Get(entity);
                var model = viewComponent.Model;
                if(model.Purchased.Value) continue;
                foreach (var balanceEntity in _balanceFilter)
                {
                    ref var balance = ref _world.GetPool<BalanceComponent>().Get(balanceEntity);
                    model.Active.Value = model.Price.Value <= balance.value;
                }
            }
        }
    }
}