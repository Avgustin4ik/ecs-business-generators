namespace GeneratorGame.Code.Ecs.Gameplay.SaveLoad
{
    using Components;
    using Leopotam.EcsLite;
    using Systems;

    public struct SaveLoadAspect
    {
        public readonly EcsFilter LoadDataRequestFilter;
        public readonly EcsPool<LoadDataRequest> LoadData;
        public readonly EcsPool<GeneratorLoaded> GeneratorLoaded;
        public readonly EcsPool<UpgradesLoaded> UpgradesLoaded;
        public readonly EcsPool<BalanceLoaded> BalanceLoaded;

        public SaveLoadAspect(EcsWorld world)
        {
            LoadDataRequestFilter = world.Filter<LoadDataRequest>().End();
            LoadData = world.GetPool<LoadDataRequest>();
            BalanceLoaded = world.GetPool<BalanceLoaded>();
            GeneratorLoaded = world.GetPool<GeneratorLoaded>();
            UpgradesLoaded = world.GetPool<UpgradesLoaded>();
        }
    }
}