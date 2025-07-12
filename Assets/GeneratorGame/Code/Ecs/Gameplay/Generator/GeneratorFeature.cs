using GeneratorGame.Code.Ecs.Gameplay.Generator;
using Leopotam.EcsLite;

public class GeneratorFeature : EcsSystems
{
    public GeneratorFeature(EcsWorld world, object shared = null) : base(world, shared)
    {
        var aspect = new GeneratorAspect(world);
        this.Add(new GenerateIncomeSystem(aspect));
        this.Add(new LevelUpGeneratorSystem(aspect));
        this.Add(new UpgradeGeneratorSystem(aspect));
    }
}