namespace GeneratorGame.Code.Ecs.Gameplay.Generator.Systems
{
    using Components;
    using Leopotam.EcsLite;

    public class UpgradeGeneratorSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly GeneratorAspect _aspect;
        private EcsFilter _filter;

        public UpgradeGeneratorSystem(GeneratorAspect aspect)
        {
            _aspect = aspect;
        }

        public void Init(IEcsSystems systems)
        {
            _filter = systems.GetWorld().Filter<UpgradeGeneratorRequest>().Inc<GeneratorComponent>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var generator = ref _aspect.Generator.Get(entity);
                ref var request = ref _aspect.Upgrade.Get(entity);
                if (request.Multiplier > 1f) request.Multiplier /= 100f;
                generator.UpgradesMultiplier += request.Multiplier;
                generator.UpdateIncome();
                _aspect.Upgrade.Del(entity);
            }
        }
    }
}