namespace GeneratorGame.Code.Ecs
{
    using System;
    using Gameplay.SaveLoad;
    using Gameplay.Generator;
    using Gameplay.Generator.Systems;
    using Gameplay.SaveLoad.Systems;
    using Ui.Systems;
    using GeneratorGame.Code.Services.Ui;
    using Leopotam.EcsLite;
    using Services.Generator;
    using UnityEngine;

    public class EcsCore : MonoBehaviour
    {
        public ApplicationQuitHandler quitHandler;
        public GeneratorDataServiceSource generatorDataServiceSource;
        public UiServiceSource uiServiceSource;
        private EcsWorld _world;
        private IEcsSystems _gameplaySystems;
        private IEcsSystems _uiSystems;
        public EcsWorld World => _world;
        public Transform uiRoot;
        private EcsSystems _debugSystems;
        private EcsSystems _saveSystems;
        private EcsSystems _loadSystems;

        public static class EcsGlobalData
        {
            public static EcsWorld World { get; private set; }

            public static void SetWorld(EcsWorld world)
            {
                World = world ?? throw new ArgumentNullException(nameof(world), "EcsWorld cannot be null");
            }
        }

        private void Awake()
        {
            _world = new EcsWorld();
            EcsGlobalData.SetWorld(_world); //для доступности в монобехах
            var generatorDataService = generatorDataServiceSource.CreateService();
            var uiService = uiServiceSource.CreateService();

            var generatorAspect = new GeneratorAspect(_world);
            var uiAspect = new UiAspect(_world);
            var saveLoadAspect = new SaveLoadAspect(_world);
            
            _saveSystems = new EcsSystems(_world);
            _saveSystems
                .Add(new SaveSystem(generatorAspect));
            
            _loadSystems = new EcsSystems(_world);
            _loadSystems
                .Add(new GenerateAndDeleteLoadRequestSystem(saveLoadAspect))
                .Add(new LoadBalanceSystem(generatorAspect,saveLoadAspect))
                .Add(new LoadGeneratorsSystem(generatorAspect, saveLoadAspect))
                .Add(new LoadUpgradesSystem(generatorAspect, saveLoadAspect));
            
            
            _uiSystems = new EcsSystems(_world);
            _uiSystems
                .Add(new SpawnGeneratorsViewSystem(generatorAspect, uiService, uiRoot))
                .Add(new UpdateProgressSystem(generatorAspect, uiAspect))
                .Add(new UpdateBalanceSystem(generatorAspect))
                .Add(new OnClickLevelUpSystem(generatorAspect, uiAspect))
                .Add(new OnClickUpgradeSystem(generatorAspect, uiAspect))
                .Add(new UpdateLevelUpButtonViewSystem(generatorAspect, uiAspect))
                .Add(new UpdateGeneratorViewSystem(generatorAspect, uiAspect))
                .Add(new UpdateUpgradeButtonViewSystem(generatorAspect, uiAspect));
            
            _gameplaySystems = new EcsSystems(_world, generatorDataService);
            _gameplaySystems
                .Add(new StartGameSystem(generatorAspect))
                .Add(new CreateGeneratorSystem(generatorAspect))
                .Add(new GenerateIncomeSystem(generatorAspect))
                .Add(new LevelUpGeneratorSystem(generatorAspect))
                .Add(new UpgradeGeneratorSystem(generatorAspect));
            
            _debugSystems = new EcsSystems(_world);
            
            quitHandler.Initialize(_world, _saveSystems);
#if UNITY_EDITOR
            _debugSystems.Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
                .Add(new Leopotam.EcsLite.UnityEditor.EcsSystemsDebugSystem());
#endif
        }

        private void Start()
        {
            _saveSystems.Init();
            _loadSystems.Init();
            _uiSystems.Init();
            _gameplaySystems.Init();
#if UNITY_EDITOR
            _debugSystems.Init();
#endif
        }

        private void Update()
        {
            _debugSystems?.Run();
            _gameplaySystems?.Run();
            _loadSystems?.Run();
            _uiSystems?.Run();
        }

        private void LateUpdate()
        {
            _loadSystems?.Run();
        }

        private void OnDestroy()
        {
            _saveSystems?.Run();
            
            _saveSystems?.Destroy();
            _loadSystems?.Destroy();
            _gameplaySystems?.Destroy();
            _uiSystems?.Destroy();
            _world?.Destroy();
            _world = null;
            _uiSystems = null;
            _gameplaySystems = null;
            _loadSystems = null;
            _saveSystems = null;
        }
    }
}