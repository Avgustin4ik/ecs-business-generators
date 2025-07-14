namespace GeneratorGame.Code.Ecs.Ui.Systems
{
    using Leopotam.EcsLite;
    using Mono;

    public struct UiAspect
    {
        public EcsFilter GeneratorViewFilter;
        public EcsPool<UIViewComponent<UIGeneratorModel>> GeneratorView;
        public EcsPool<LinkedGeneratorComponent> GeneratorLinked;
        
        public UiAspect(EcsWorld world)
        {
            GeneratorViewFilter = world.Filter<UIViewComponent<UIGeneratorModel>>().End();
            GeneratorView = world.GetPool<UIViewComponent<UIGeneratorModel>>();
            GeneratorLinked = world.GetPool<LinkedGeneratorComponent>();
            UpgradeButton = world.GetPool<UIViewComponent<UpgradeButtonModel>>();
        }

        public EcsPool<UIViewComponent<UpgradeButtonModel>> UpgradeButton;
    }
}