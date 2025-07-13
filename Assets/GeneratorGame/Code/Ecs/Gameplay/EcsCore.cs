namespace GeneratorGame.Code.Ecs.Gameplay
{
    using System;
    using System.Threading;
    using Generator;
    using Leopotam.EcsLite;
    using Services;
    using Services.Ui;
    using Ui;
    using Ui.Systems;
    using UnityEngine;

    public class EcsCore : MonoBehaviour
    {
        public GeneratorDataServiceSource GeneratorDataServiceSource;
        public UiServiceSource UiServiceSource;
        private EcsWorld _world;
        private IDisposable _worldDisposable;
        private IEcsSystems _updateSystems;
        private IEcsSystems _fixedSystems;
        private IEcsSystems _lateSystems;
        private UiFeature _uiSystems;
        public EcsWorld World => _world;
        public IDisposable WorldDisposable => _worldDisposable;
    
        public Transform UiRoot;
        private EcsSystems _debugSystems;

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
            
            _worldDisposable = new CancellationTokenSource();
            var generatorDataService = GeneratorDataServiceSource.CreateService();
            var uiService = UiServiceSource.CreateService();

            _uiSystems = new UiFeature(_world);//TODO добавить возможность добавления нескольких Shaerd объектов в системы (фичи)
            _uiSystems.Add(new SpawnGeneratorsViewSystem(uiService,generatorDataService, UiRoot));
            _uiSystems.Add(new UpdateProgressSystem());
            _uiSystems.Add(new UpdateBalanceSystem());
            // _uiSystems.Add(new InitializeUiSystem());
            
            _updateSystems = new GeneratorFeature(_world, generatorDataService);
            _fixedSystems = new EcsSystems(_world);
            _lateSystems = new EcsSystems(_world);
            
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
            _uiSystems.Init();
            _updateSystems.Init();
            _fixedSystems.Init();
            _lateSystems.Init();
            
#if UNITY_EDITOR
            // Инициализируем отладочные системы:
            _debugSystems.Init();
#endif
        }
        
        private void Update()
        {
            _debugSystems?.Run();
            _updateSystems?.Run();
            _uiSystems?.Run();
        }
        
        private void FixedUpdate()
        {
            _fixedSystems?.Run();
        }
        
        private void LateUpdate()
        {
            _lateSystems?.Run();
        }
        
        private void OnDestroy()
        {
            _updateSystems?.Destroy();
            _fixedSystems?.Destroy();
            _lateSystems?.Destroy();
            _worldDisposable?.Dispose();
            _uiSystems?.Destroy();
            _world?.Destroy();
            _world = null;
            _uiSystems = null;
            _worldDisposable = null;
            _updateSystems = null;
            _fixedSystems = null;
            _lateSystems = null;
        }
    }
}