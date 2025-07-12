namespace GeneratorGame.Code.Ecs.Gameplay.Generator
{
    using Leopotam.EcsLite;

    public class UpgradeGeneratorSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly GeneratorAspect _aspect;
        private EcsWorld _world;
        private EcsFilter _filter;

        public UpgradeGeneratorSystem(GeneratorAspect aspect)
        {
            _aspect = aspect;
        }

        public void Init(IEcsSystems systems)
        {
            _filter = systems.GetWorld().Filter<UpgradeGeneratorSelfEvent>().Inc<GeneratorComponent>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var generator = ref _aspect.Generator.Get(entity);
                ref var upgradeEvent = ref _aspect.Upgrade.Get(entity);
                generator.UpgradesMultiplier += upgradeEvent.Multiplier;
            }
        }
    }
}