using GeneratorGame.Code.Ecs.Gameplay.Player;
using Leopotam.EcsLite;

namespace GeneratorGame.Code.Ecs.Gameplay.Generator
{
    using System.Collections;
    using Components;
    using Ui.Systems;

    public struct GeneratorAspect
    {
        public EcsPool<GeneratorComponent> Generator;
        public EcsPool<BalanceComponent> Balance;
        public EcsPool<UpgradeGeneratorRequest> Upgrade;
        public EcsPool<LevelUpGeneratorRequest> LevelUp;
        public EcsFilter GeneratorFilter;
        public EcsFilter BalanceFilter;
        public EcsPool<LevelUpPriceComponent> LevelUpPrice;
    
        public GeneratorAspect(EcsWorld world)
        {
            Generator = world.GetPool<GeneratorComponent>();
            Balance = world.GetPool<BalanceComponent>();
            GeneratorFilter = world.Filter<GeneratorComponent>().End();
            BalanceFilter = world.Filter<BalanceComponent>().End();
            Upgrade = world.GetPool<UpgradeGeneratorRequest>();
            LevelUp = world.GetPool<LevelUpGeneratorRequest>();
            LevelUpPrice = world.GetPool<LevelUpPriceComponent>();
            AvailableUpgrade = world.GetPool<AvailableUpgradeComponent>();
            AvailableUpgradeFilter = world.Filter<AvailableUpgradeComponent>().End();
            Purchased = world.GetPool<PurchasedFlagComponent>();
        }

        public EcsPool<AvailableUpgradeComponent> AvailableUpgrade;
        public EcsFilter AvailableUpgradeFilter;
        public EcsPool<PurchasedFlagComponent> Purchased;
    }
}