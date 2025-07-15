namespace GeneratorGame.Code.Ecs.Gameplay.SaveLoad.Systems
{
    using GeneratorGame.Code.Ecs.Gameplay.Generator;
    using Leopotam.EcsLite;
    using UnityEngine;

    public class LoadBalanceSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly GeneratorAspect _aspect;
        private readonly SaveLoadAspect _saveLoadAspect;
        private EcsWorld _world;

        public LoadBalanceSystem(GeneratorAspect aspect, SaveLoadAspect saveLoadAspect)
        {
            _aspect = aspect;
            _saveLoadAspect = saveLoadAspect;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
        }

        public void Run(IEcsSystems systems)
        {
            if(_saveLoadAspect.LoadDataRequestFilter.GetEntitiesCount() == 0) return;
            Debug.Log("Balance Load System Run");
            _saveLoadAspect.BalanceLoaded.Add(_world.NewEntity());
            foreach (var e in _aspect.BalanceFilter)
            {
                ref var balance = ref _aspect.Balance.Get(e);
                balance.value = PlayerPrefs.GetFloat(SaveKeys.BALANCE, 0);
            }
        }
    }
}