namespace GeneratorGame.Code.Ecs
{
    using System;
    using Gameplay.SaveLoad;
    using GeneratorGame.Code.Ecs.Gameplay.Generator;
    using GeneratorGame.Code.Ecs.Gameplay.Generator.Systems;
    using GeneratorGame.Code.Ecs.Ui.Systems;
    using GeneratorGame.Code.Services;
    using GeneratorGame.Code.Services.Ui;
    using Leopotam.EcsLite;
    using Services.Generator;
    using UnityEngine;

    public class EcsCore : MonoBehaviour
    {
        public GeneratorDataServiceSource generatorDataServiceSource;
        public UiServiceSource uiServiceSource;
        private EcsWorld _world;
        private IEcsSystems _gameplaySystems;
        private IEcsSystems _uiSystems;
        public EcsWorld World => _world;
        public Transform uiRoot;
        private EcsSystems _debugSystems;
        private EcsSystems _saveLoadSystems;

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

            _saveLoadSystems = new EcsSystems(_world);
            _saveLoadSystems
                .Add(new SaveSystem(generatorAspect))
                .Add(new LoadSystem(generatorAspect));
            _uiSystems = new EcsSystems(_world);
            _uiSystems
                .Add(new SpawnGeneratorsViewSystem(generatorAspect, uiService, uiRoot))
                .Add(new UpdateProgressSystem(generatorAspect, uiAspect))
                .Add(new UpdateBalanceSystem())
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
#if UNITY_EDITOR
            // Регистрируем отладочные системы по контролю за состоянием каждого отдельного мира:
            // .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ("events"))
            _debugSystems.Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
                // Регистрируем отладочные системы по контролю за текущей группой систем. 
                .Add(new Leopotam.EcsLite.UnityEditor.EcsSystemsDebugSystem());
#endif
        }

        private void Start()
        {
            _saveLoadSystems.Init();
            _uiSystems.Init();
            _gameplaySystems.Init();
#if UNITY_EDITOR
            // Инициализируем отладочные системы:
            _debugSystems.Init();
#endif
        }

        private void Update()
        {
            _debugSystems?.Run();
            _gameplaySystems?.Run();
            _saveLoadSystems?.Run();
            _uiSystems?.Run();
        }
        private void OnDestroy()
        {
            _saveLoadSystems?.Run();
            _saveLoadSystems?.Destroy();
            _gameplaySystems?.Destroy();
            _uiSystems?.Destroy();
            _world?.Destroy();
            _world = null;
            _uiSystems = null;
            _gameplaySystems = null;
            _saveLoadSystems = null;
        }
    }
}