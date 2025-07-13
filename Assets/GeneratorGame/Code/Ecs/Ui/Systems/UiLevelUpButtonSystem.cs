namespace GeneratorGame.Code.Ecs.Ui
{
    using Gameplay.Generator;
    using Leopotam.EcsLite;
    using Mono;
    using Services;

    public class UiLevelUpButtonSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        private EcsFilter _filter;
        private EcsPool<UIViewComponent<UIGeneratorView>> _viewPool;
        private EcsPool<UiInitiazliedFlagComponent> _initFlagPool;
        private EcsWorld _world;
        private EcsPool<LevelUpGeneratorSelfEvent> _levelUpEventPool;
        private EcsPool<UpgradeGeneratorSelfEvent> _upgradeEventPool;
        private IGeneratorDataService _generatorDataService;

        public void Init(IEcsSystems systems)
        {
            _generatorDataService = systems.GetShared<IGeneratorDataService>();
            _world = systems.GetWorld();
            _filter = _world.Filter<UIViewComponent<UIGeneratorView>>().Inc<GeneratorComponent>().Exc<UiInitiazliedFlagComponent>()
                .End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var view in _filter)
            {
                ref var uiView = ref _viewPool.Get(view);
                if (uiView.Type != typeof(UIGeneratorView)) continue;
                var uiGeneratorView = (UIGeneratorView)uiView.View;
                ref var generator = ref _world.GetPool<GeneratorComponent>().Get(view);
                
                if (uiGeneratorView.LevelUpClicked.Value)
                {
                    _levelUpEventPool.Add(view);
                    uiGeneratorView.LevelUpClicked.Value = false;
                }
                
                _initFlagPool.Add(view);
            }
        }

        public void Destroy(IEcsSystems systems)
        {
            throw new System.NotImplementedException();
        }
    }
    
}