namespace GeneratorGame.Code.Ecs.Ui.Systems
{
    using Components;
    using Leopotam.EcsLite;
    using Mono;

    public struct UiAspect
    {
        public readonly EcsFilter GeneratorViewFilter;
        public readonly EcsPool<UIViewComponent<UIGeneratorModel>> GeneratorView;
        public readonly EcsPool<LinkedGeneratorComponent> GeneratorLinked;
        public readonly EcsPool<UIViewComponent<UpgradeButtonModel>> UpgradeButton;

        public UiAspect(EcsWorld world)
        {
            GeneratorViewFilter = world.Filter<UIViewComponent<UIGeneratorModel>>().End();
            GeneratorView = world.GetPool<UIViewComponent<UIGeneratorModel>>();
            GeneratorLinked = world.GetPool<LinkedGeneratorComponent>();
            UpgradeButton = world.GetPool<UIViewComponent<UpgradeButtonModel>>();
        }
    }
}