namespace GeneratorGame.Code.Ecs.Ui.Systems
{
    using System.Collections.Generic;
    using System.Linq;
    using Components;
    using Cysharp.Threading.Tasks;
    using Mono;
    using Services;
    using GeneratorGame.Code.Services.Ui;
    using Leopotam.EcsLite;
    using UnityEngine;

    public class SpawnGeneratorsViewSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsPool<GeneratorGuid> _generatorGuidPool;
        private readonly IUiService _uiConfig;
        private readonly IGeneratorDataService _config;
        private readonly Transform _root;
        private EcsFilter _filter;

        public SpawnGeneratorsViewSystem(IUiService uiService, GeneratorDataService generatorDataService, Transform uiRoot = null)
        {
            _root = uiRoot;
            _uiConfig = uiService;
            _config = generatorDataService;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _generatorGuidPool = _world.GetPool<GeneratorGuid>();
            _filter = _world.Filter<GeneratorLoadedComponent>().End();
        }
        
        public void Run(IEcsSystems systems)
        {
            if(_filter.GetEntitiesCount() > 0) return;
            SpawnGeneratorAsync(_config.GetAllGenerators()).Forget();
            _world.GetPool<GeneratorLoadedComponent>().Add(_world.NewEntity());
        }

        private async UniTaskVoid SpawnGeneratorAsync(IEnumerable<GeneratorData> data)
        {
            await UniTask.WhenAll(data.Select(Spawn));
        }

        private async UniTask Spawn(GeneratorData data)
        {
            var task = Object.InstantiateAsync<UIGeneratorView>(_uiConfig.GeneratorPrefab, _root);
            task.WaitForCompletion();
            var uiGeneratorView = task.Result.First();
            InitializeEntity(uiGeneratorView, data.Guid);
        }

        private void InitializeEntity(UIGeneratorView view, string dataGuid) 
        {
            var entity = _world.NewEntity();
            view.ApplyEcsWorld<UIGeneratorView>(_world, entity);
            ref var generatorGuid = ref _generatorGuidPool.Add(entity);
            generatorGuid.Guid = dataGuid;
        }
    }
}