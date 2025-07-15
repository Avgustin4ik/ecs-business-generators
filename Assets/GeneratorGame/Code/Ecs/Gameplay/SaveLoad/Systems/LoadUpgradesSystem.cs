namespace GeneratorGame.Code.Ecs.Gameplay.SaveLoad.Systems
{
    using System.Collections.Generic;
    using GeneratorGame.Code.Ecs.Gameplay.Generator;
    using Leopotam.EcsLite;
    using UnityEngine;

    public class LoadUpgradesSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly GeneratorAspect _aspect;
        private readonly SaveLoadAspect _saveLoadAspect;
        private EcsWorld _world;

        public LoadUpgradesSystem(GeneratorAspect aspect, SaveLoadAspect saveLoadAspect)
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
            
            var upgradesJson = PlayerPrefs.GetString(SaveKeys.UPGRADES,string.Empty);
            if (string.IsNullOrEmpty(upgradesJson))
            {
#if UNITY_EDITOR
                Debug.Log($"LoadSystem: upgrades data not found");
#endif
                _saveLoadAspect.UpgradesLoaded.Add(_world.NewEntity());

                return;
            }
            var upgradesSaveHashSet = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(upgradesJson);

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
                
                _saveLoadAspect.UpgradesLoaded.Add(_world.NewEntity());
            }
        }
    }
}
