namespace GeneratorGame.Code.Ecs.Gameplay.SaveLoad
{
    using System.Collections.Generic;
    using Generator;
    using Leopotam.EcsLite;
    using UnityEngine;

    public class SaveSystem : IEcsDestroySystem
    {
        private readonly GeneratorAspect _aspect;

        public SaveSystem(GeneratorAspect aspect)
        {
            _aspect = aspect;
        }
        
        public void Destroy(IEcsSystems systems)
        {
            SaveBusiness();
            SaveUpgrades();
        }

        private void SaveUpgrades()
        {
            var set = new HashSet<string>();
            foreach (var upgradeEntity in _aspect.AvailableUpgradeFilter)
            {
                ref var upgrade = ref _aspect.AvailableUpgrade.Get(upgradeEntity);
                if(!_aspect.Purchased.Has(upgradeEntity)) continue;
                set.Add(upgrade.Guid);
            }
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(set);
            PlayerPrefs.SetString("upgrades", json);
        }

        private void SaveBusiness()
        {
            foreach (var e in _aspect.BalanceFilter)
            {
                ref var balance = ref _aspect.Balance.Get(e);
                PlayerPrefs.SetFloat("balance",balance.value);
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
#if UNITY_EDITOR
            Debug.Log("SaveSystem done");
#endif
        }
        public class GeneratorSaveData
        {
            public int Level;
            public float Progress;
        }
    }
}
