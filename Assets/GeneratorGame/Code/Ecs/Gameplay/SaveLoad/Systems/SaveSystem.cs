namespace GeneratorGame.Code.Ecs.Gameplay.SaveLoad.Systems
{
    using System.Collections.Generic;
    using GeneratorGame.Code.Ecs.Gameplay.Generator;
    using Leopotam.EcsLite;
    using UnityEngine;

    public class SaveSystem : IEcsDestroySystem, IEcsRunSystem
    {
        private readonly GeneratorAspect _aspect;

        public SaveSystem(GeneratorAspect aspect)
        {
            _aspect = aspect;
        }
        
        public void Run()
        {
            SaveBusiness();
            SaveUpgrades();
        }
        private void SaveUpgrades()
        {
            var set = new List<string>();
            foreach (var upgradeEntity in _aspect.AvailableUpgradeFilter)
            {
                ref var upgrade = ref _aspect.AvailableUpgrade.Get(upgradeEntity);
                if(!_aspect.Purchased.Has(upgradeEntity)) continue;
                set.Add(upgrade.Guid);
            }
            if(set.Count == 0) return;
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(set);
            PlayerPrefs.SetString(SaveKeys.UPGRADES, json);
        }

        private void SaveBusiness()
        {
            foreach (var e in _aspect.BalanceFilter)
            {
                ref var balance = ref _aspect.Balance.Get(e);
                PlayerPrefs.SetFloat(SaveKeys.BALANCE,balance.value);
                PlayerPrefs.Save();
            }
            foreach (var e in _aspect.GeneratorFilter)
            {
                ref var generator = ref _aspect.Generator.Get(e);
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(new GeneratorSaveData
                {
                    Level = generator.Level,
                    Progress = generator.Progress
                });
                PlayerPrefs.SetString(generator.Guid,json);
            }
            PlayerPrefs.Save();
            Debug.Log("SaveSystem done");
        }
        public class GeneratorSaveData
        {
            public int Level;
            public float Progress;
        }

        public void Destroy(IEcsSystems systems)
        {
            Run();
        }

        public void Run(IEcsSystems systems)
        {
            Run();
        }
    }
}
