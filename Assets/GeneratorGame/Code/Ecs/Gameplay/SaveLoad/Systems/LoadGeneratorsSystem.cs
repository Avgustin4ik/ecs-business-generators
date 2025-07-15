namespace GeneratorGame.Code.Ecs.Gameplay.SaveLoad.Systems
{
    using GeneratorGame.Code.Ecs.Gameplay.Generator;
    using Leopotam.EcsLite;
    using UnityEngine;

    public struct LoadDataRequest
    {
        
    }
    
    public class LoadGeneratorsSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly GeneratorAspect _aspect;
        private readonly SaveLoadAspect _saveLoadAspect;
        private EcsWorld _world;

        public LoadGeneratorsSystem(GeneratorAspect aspect, SaveLoadAspect saveLoadAspect)
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
            foreach (var e in _aspect.GeneratorFilter)
            {
                ref var generator = ref _aspect.Generator.Get(e);
                var json = PlayerPrefs.GetString(generator.Guid, string.Empty);
                if (string.IsNullOrEmpty(json))
                {
                    Debug.Log($"GeneratorSaveData with guid {generator.Guid} not found");
                    continue;
                }
                var data = Newtonsoft.Json.JsonConvert.DeserializeObject<SaveSystem.GeneratorSaveData>(json);
                
                generator.Progress = data.Progress;
                generator.Level = data.Level;
                generator.UpdateIncome();
                
                ref var lvlupPrice = ref _aspect.LevelUpPrice.Get(e);
                lvlupPrice.UpdatePrice(generator.Level);
            }
            _saveLoadAspect.GeneratorLoaded.Add(_world.NewEntity());
        }
    }
}