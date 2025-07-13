namespace GeneratorGame.Code.Ecs.Gameplay.Generator
{
    using System;
    using Leopotam.EcsLite;
    using Player;

    public class GenerateIncomeSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private readonly GeneratorAspect _aspect;

        public GenerateIncomeSystem(GeneratorAspect aspect)
        {
            _aspect = aspect;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _aspect.GeneratorFilter)
            {
                ref var generator = ref _aspect.Generator.Get(entity);
                
                if (DateTime.UtcNow < generator.NextIncomeTime) continue;
                
                generator.NextIncomeTime = DateTime.UtcNow.AddSeconds(generator.DurationInSeconds);
                ref var balance = ref _aspect.Balance.Get(entity);
                balance.value += generator.Level * generator.BaseIncome * (1f + generator.UpgradesMultiplier);
            }
        }
    }
}