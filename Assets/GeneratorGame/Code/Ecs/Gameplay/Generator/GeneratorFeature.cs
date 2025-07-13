using Leopotam.EcsLite;

namespace GeneratorGame.Code.Ecs.Gameplay.Generator
{
    using Systems;

    public class GeneratorFeature : EcsSystems
    {
        public GeneratorFeature(EcsWorld world, object shared = null) : base(world, shared)
        {
            var aspect = new GeneratorAspect(world);
            this.Add(new StartGameSystem(aspect));
            this.Add(new CreateGeneratorSystem(aspect));
            this.Add(new GenerateIncomeSystem(aspect));
            this.Add(new LevelUpGeneratorSystem(aspect));
            this.Add(new UpgradeGeneratorSystem(aspect));
        }
    }
}