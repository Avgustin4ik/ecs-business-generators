namespace GeneratorGame.Code.Ecs.Ui.Systems
{
    using Gameplay.Player;
    using Leopotam.EcsLite;
    using Mono;

    public class UpdateBalanceSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _viewFilter;
        private EcsPool<BalanceComponent> _balancePool;
        private EcsFilter _balanceFilter;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _viewFilter = _world.Filter<UIViewComponent<UIBalanceModel>>().End();
            _balancePool = _world.GetPool<BalanceComponent>();
            _balanceFilter = _world.Filter<BalanceComponent>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var viewEntity in _viewFilter)
            {
                ref var viewComponent = ref _world.GetPool<UIViewComponent<UIBalanceModel>>().Get(viewEntity);
                var model = viewComponent.Model;

                foreach (var balanceEntity in _balanceFilter)
                {
                    ref var balanceComponent = ref _world.GetPool<BalanceComponent>().Get(balanceEntity);
                    model.Balance.Value = balanceComponent.value;
                }
            }
        }
    }
}