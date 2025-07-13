namespace GeneratorGame.Code.Ecs.Ui.Systems
{
    using Gameplay.Player;
    using Leopotam.EcsLite;
    using Mono;
    using UnityEngine;

    public class UpdateBalanceSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _viewFilter;
        private EcsPool<BalanceComponent> _balancePool;
        private EcsFilter _balanceFilter;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _viewFilter = _world.Filter<UIViewComponent<UIBalanceView>>().End();
            _balancePool = _world.GetPool<BalanceComponent>(); // Ensure BalanceComponent is created
            _balanceFilter = _world.Filter<BalanceComponent>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var viewEntity in _viewFilter)
            {
                ref var viewComponent = ref _world.GetPool<UIViewComponent<UIBalanceView>>().Get(viewEntity);
                var viewComponentView = viewComponent.View as UIBalanceView;
                if (viewComponentView == null) continue;

                foreach (var balanceEntity in _balanceFilter)
                {
                    ref var balanceComponent = ref _world.GetPool<BalanceComponent>().Get(balanceEntity);
                    viewComponentView.SetBalanceText(balanceComponent.value);
                }
            }
        }
    }
}