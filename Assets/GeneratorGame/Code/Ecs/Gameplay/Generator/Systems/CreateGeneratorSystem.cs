namespace GeneratorGame.Code.Ecs.Gameplay.Generator
{
    using System;
    using System.Linq;
    using Leopotam.EcsLite;
    using Services;

    public class CreateGeneratorSystem : IEcsInitSystem
    {
        private readonly GeneratorAspect _aspect;
        private IGeneratorDataService _config;

        public CreateGeneratorSystem(GeneratorAspect aspect)
        {
            _aspect = aspect;
        }

        public void Init(IEcsSystems systems)
        {
            _config = systems.GetShared<IGeneratorDataService>();
            var world = systems.GetWorld();
            var data = _config.GetAllGenerators().FirstOrDefault();
            var entity = world.NewEntity();
            ref var generator = ref _aspect.Generator.Add(entity);
            generator.Guid = data.Guid;
            generator.Level = 1;
            generator.BaseIncome = data.Income;
            generator.DurationInSeconds = data.DurationInSeconds;
            generator.UpgradesMultiplier = 0f;

            // Initialize next income time
            generator.NextIncomeTime = DateTime.UtcNow.AddSeconds(data.DurationInSeconds);
        }
    }
}