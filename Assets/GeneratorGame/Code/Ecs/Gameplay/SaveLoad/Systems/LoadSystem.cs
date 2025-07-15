namespace GeneratorGame.Code.Ecs.Gameplay.SaveLoad.Systems
{
    using System.Collections.Generic;
    using Components;
    using GeneratorGame.Code.Ecs.Gameplay.Generator;
    using Leopotam.EcsLite;
    using UnityEngine;

    public class LoadSystem : IEcsInitSystem, IEcsPostRunSystem
    {
        private readonly GeneratorAspect _aspect;
        private EcsFilter _filter;
        private EcsPool<GameLoadedFlagComponent> _pool;

        public LoadSystem(GeneratorAspect aspect)
        {
            _aspect = aspect;
        }
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _filter = world.Filter<GameLoadedFlagComponent>().End();
            _pool = world.GetPool<GameLoadedFlagComponent>();
        }

        public void PostRun(IEcsSystems systems)
        {
            if(_filter.GetEntitiesCount()>0) return;
            _pool.Add(systems.GetWorld().NewEntity());

            foreach (var e in _aspect.BalanceFilter)
            {
                ref var balance = ref _aspect.Balance.Get(e);
                balance.value = PlayerPrefs.GetFloat(SaveKeys.BALANCE, 0);
            }
            
            foreach (var e in _aspect.GeneratorFilter)
            {
                ref var generator = ref _aspect.Generator.Get(e);
                var json = PlayerPrefs.GetString(generator.Guid, string.Empty);
                if (string.IsNullOrEmpty(json))
                {
#if UNITY_EDITOR
                    Debug.Log($"GeneratorSaveData with guid {generator.Guid} not found");
#endif
                    continue;
                }
                var data = Newtonsoft.Json.JsonConvert.DeserializeObject<SaveSystem.GeneratorSaveData>(json);
                
                generator.Progress = data.Progress;
                generator.Level = data.Level;
                generator.UpdateIncome();
                
                ref var lvlupPrice = ref _aspect.LevelUpPrice.Get(e);
                lvlupPrice.UpdatePrice(generator.Level);
            }

            var upgradesJson = PlayerPrefs.GetString(SaveKeys.UPGRADES,string.Empty);
            
            
            if (string.IsNullOrEmpty(upgradesJson))
            {
#if UNITY_EDITOR
                Debug.Log($"LoadSystem: upgrades data not found");
#endif
                return;
            }
            
            var upgradesSaveHashSet = Newtonsoft.Json.JsonConvert.DeserializeObject<HashSet<string>>(upgradesJson);

            foreach (var e in _aspect.AvailableUpgradeFilter)
            {
                ref var upgrade = ref _aspect.AvailableUpgrade.Get(e);
                if (!upgradesSaveHashSet.Contains(upgrade.Guid)) continue;
                if (_aspect.Purchased.Has(e)) continue;
                _aspect.Purchased.Add(e);
                
                foreach (var genEntity in _aspect.GeneratorFilter)
                {
                    ref var generator = ref _aspect.Generator.Get(genEntity);
                    if(generator.Guid != upgrade.GeneratorGuid) continue;
                    ref var upgradeRequest = ref _aspect.Upgrade.Add(systems.GetWorld().NewEntity());
                    upgradeRequest.Multiplier = upgrade.Multiplayer;
                    upgradeRequest.generatorGuid = generator.Guid;
                }
            }

            _pool.Add(systems.GetWorld().NewEntity());
        }
    }
}