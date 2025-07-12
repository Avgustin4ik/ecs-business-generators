using GeneratorGame.Code.Ecs.Gameplay.Player;
using Leopotam.EcsLite;

namespace GeneratorGame.Code.Ecs.Gameplay.Generator
{
    public struct GeneratorAspect
    {
        public EcsPool<GeneratorComponent> Generator;
        public EcsPool<BalanceComponent> Balance;
        public EcsPool<UpgradeGeneratorSelfEvent> Upgrade;
        public EcsPool<LevelUpGeneratorSelfEvent> LevelUp;
        public EcsFilter GeneratorFilter;
        public EcsFilter BalanceFilter;
    
        public GeneratorAspect(EcsWorld world)
        {
            Generator = world.GetPool<GeneratorComponent>();
            Balance = world.GetPool<BalanceComponent>();
            GeneratorFilter = world.Filter<GeneratorComponent>().End();
            BalanceFilter = world.Filter<BalanceComponent>().End();
            Upgrade = world.GetPool<UpgradeGeneratorSelfEvent>();
            LevelUp = world.GetPool<LevelUpGeneratorSelfEvent>();
        }

    }
}