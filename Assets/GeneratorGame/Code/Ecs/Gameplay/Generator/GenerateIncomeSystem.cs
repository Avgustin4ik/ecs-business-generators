namespace GeneratorGame.Code.Ecs.Gameplay.Generator
{
    using System;
    using Leopotam.EcsLite;
    using Player;

    public class GenerateIncomeSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<GeneratorComponent> _generatorPool;
        private EcsPool<BalanceComponent> _balancePool;
        
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<GeneratorComponent>().End();
            _generatorPool = _world.GetPool<GeneratorComponent>();
            _balancePool = _world.GetPool<BalanceComponent>();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var generator = ref _generatorPool.Get(entity);
                
                if (DateTime.UtcNow < generator.NextIncomeTime) continue;
                
                ref var balance = ref _balancePool.Get(entity);
                balance.value += generator.Income * generator.Level;
                generator.NextIncomeTime = DateTime.UtcNow.AddSeconds(generator.DurationInSeconds);
            }
        }
    }
}