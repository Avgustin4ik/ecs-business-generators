namespace GeneratorGame.Code.Ecs.Ui.Systems
{
    using Components;
    using Gameplay.Generator;
    using Leopotam.EcsLite;
    using Mono;

    public class UpdateBalanceSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly GeneratorAspect _aspect;
        private EcsWorld _world;
        private EcsFilter _viewFilter;
        private EcsPool<UIViewComponent<UIBalanceModel>> _balanceViewPool;

        public UpdateBalanceSystem(GeneratorAspect aspect)
        {
            _aspect = aspect;
        }
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _viewFilter = _world.Filter<UIViewComponent<UIBalanceModel>>().End();
            _balanceViewPool = _world.GetPool<UIViewComponent<UIBalanceModel>>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var viewEntity in _viewFilter)
            {
                ref var viewComponent = ref _balanceViewPool.Get(viewEntity);
                var model = viewComponent.Model;

                foreach (var balanceEntity in _aspect.BalanceFilter)
                {
                    ref var balanceComponent = ref _aspect.Balance.Get(balanceEntity);
                    model.Balance.Value = balanceComponent.value;
                }
            }
        }
    }
}