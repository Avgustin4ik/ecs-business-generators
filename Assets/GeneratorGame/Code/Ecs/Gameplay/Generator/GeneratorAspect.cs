namespace GeneratorGame.Code.Ecs.Gameplay.Generator
{
    using GeneratorGame.Code.Ecs.Gameplay.Player;
    using Leopotam.EcsLite;
    using Components;

    public struct GeneratorAspect
    {
        public EcsPool<GeneratorComponent> Generator;
        public EcsPool<BalanceComponent> Balance;
        public EcsPool<UpgradeGeneratorRequest> Upgrade;
        public EcsPool<LevelUpGeneratorSelfRequest> LevelUp;
        public EcsPool<LevelUpPriceComponent> LevelUpPrice;
        public EcsPool<AvailableUpgradeComponent> AvailableUpgrade;
        public EcsPool<PurchasedFlagComponent> Purchased;
        public EcsFilter AvailableUpgradeFilter;
        public EcsFilter GeneratorFilter;
        public EcsFilter BalanceFilter;

        public GeneratorAspect(EcsWorld world)
        {
            Generator = world.GetPool<GeneratorComponent>();
            Balance = world.GetPool<BalanceComponent>();
            GeneratorFilter = world.Filter<GeneratorComponent>().End();
            BalanceFilter = world.Filter<BalanceComponent>().End();
            Upgrade = world.GetPool<UpgradeGeneratorRequest>();
            LevelUp = world.GetPool<LevelUpGeneratorSelfRequest>();
            LevelUpPrice = world.GetPool<LevelUpPriceComponent>();
            AvailableUpgrade = world.GetPool<AvailableUpgradeComponent>();
            AvailableUpgradeFilter = world.Filter<AvailableUpgradeComponent>().End();
            Purchased = world.GetPool<PurchasedFlagComponent>();
        }

    }
}