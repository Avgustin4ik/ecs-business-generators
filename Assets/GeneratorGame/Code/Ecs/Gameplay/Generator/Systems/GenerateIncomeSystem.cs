﻿namespace GeneratorGame.Code.Ecs.Gameplay.Generator.Systems
{
    using Leopotam.EcsLite;
    using UnityEngine;

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
                if (generator.Level == 0) continue;
                
                generator.Progress += Time.deltaTime;
                if (generator.DurationInSeconds > generator.Progress) continue;
                generator.Progress = 0;
                
                foreach (var balanceEntity in _aspect.BalanceFilter)
                {
                    ref var balance = ref _aspect.Balance.Get(balanceEntity);
                    balance.value += generator.CurrentIncome;
                }
            }
        }
    }
}