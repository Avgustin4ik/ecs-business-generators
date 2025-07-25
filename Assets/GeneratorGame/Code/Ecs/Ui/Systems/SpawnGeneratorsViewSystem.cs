namespace GeneratorGame.Code.Ecs.Ui.Systems
{
    using Components;
    using Cysharp.Threading.Tasks;
    using Gameplay.Generator;
    using Gameplay.Generator.Components;
    using Mono;
    using GeneratorGame.Code.Services.Ui;
    using Leopotam.EcsLite;
    using UnityEngine;

    public class SpawnGeneratorsViewSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private readonly IUiService _uiConfig;
        private readonly Transform _root;
        private EcsFilter _filter;
        private readonly GeneratorAspect _generatorAspect;
        private EcsPool<GeneratorLoadedComponent> _loadedPool;

        public SpawnGeneratorsViewSystem(GeneratorAspect genAspect, IUiService uiService, Transform uiRoot = null)
        {
            _generatorAspect = genAspect;
            _root = uiRoot;
            _uiConfig = uiService;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<GeneratorLoadedComponent>().End();
            _loadedPool = _world.GetPool<GeneratorLoadedComponent>();
        }
        
        public void Run(IEcsSystems systems)
        {
            if(_filter.GetEntitiesCount() > 0) return;
            foreach (var generatorEntity in _generatorAspect.GeneratorFilter)
            {
                ref var generator = ref _generatorAspect.Generator.Get(generatorEntity);
                SpawnAndLinkAsync(_world.PackEntity(generatorEntity),generator.Guid);
            }

            foreach (var availableUpgradeEntity in _world.Filter<AvailableUpgradeComponent>().Inc<GeneratorComponent>().End())
            {
                ref var u = ref _generatorAspect.AvailableUpgrade.Get(availableUpgradeEntity);
                Debug.Log("AvailableUpgrade entity Detected");
            }
            _loadedPool.Add(_world.NewEntity());
        }

        private async UniTaskVoid SpawnAndLinkAsync(EcsPackedEntity packedEntity, string guid)
        {
            await Spawn(packedEntity, guid);
        }

        private async UniTask Spawn(EcsPackedEntity entityToLink, string guid)
        {
            var task = Object.InstantiateAsync<UIGeneratorView>(_uiConfig.GeneratorPrefab, _root);
            task.WaitForCompletion();
            var uiGeneratorView = task.Result[0];
            InitializeEntity(uiGeneratorView, entityToLink, guid);
            SpawnUpgrades(uiGeneratorView.Model, entityToLink);
        }

        private void SpawnUpgrades(UIGeneratorModel model, EcsPackedEntity linkedEntity)
        {
            foreach (var upgradeEntity in _generatorAspect.AvailableUpgradeFilter)
            {
                ref var component = ref _generatorAspect.AvailableUpgrade.Get(upgradeEntity);
                
                if (!linkedEntity.Unpack(_world, out var entity)) continue;
                
                ref var generatorComponent = ref _generatorAspect.Generator.Get(entity);
                if(generatorComponent.Guid != component.GeneratorGuid) continue;
                model.spawn.Execute(component.Guid);
            }
        }

        private void InitializeEntity(UIGeneratorView view, EcsPackedEntity entityToLink, string guid) 
        {
            var entity = _world.NewEntity();
            view.ApplyEcsWorld(_world, entity);
            ref var link = ref _world.GetPool<LinkedGeneratorComponent>().Add(entity);
            link.Entity = entityToLink;
        }
    }
}