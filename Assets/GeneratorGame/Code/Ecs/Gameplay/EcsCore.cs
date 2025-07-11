namespace GeneratorGame.Code.Ecs.Gameplay
{
    using System;
    using System.Threading;
    using Leopotam.EcsLite;
    using UnityEngine;

    public class EcsCore : MonoBehaviour
    {
        private EcsWorld _world;
        private IDisposable _worldDisposable;
        private IEcsSystems _systems;
        private IEcsSystems _fixedSystems;
        private IEcsSystems _lateSystems;
        public EcsWorld World => _world;
        public IDisposable WorldDisposable => _worldDisposable;
        
        private void Awake()
        {
            _world = new EcsWorld();
            _worldDisposable = new CancellationTokenSource();
            _systems = new EcsSystems(_world);
            _fixedSystems = new EcsSystems(_world);
            _lateSystems = new EcsSystems(_world);
        }
        
        private void Start()
        {
        }
        
        private void Update()
        {
            _systems?.Run();
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
            _systems?.Destroy();
            _fixedSystems?.Destroy();
            _lateSystems?.Destroy();
            _worldDisposable?.Dispose();
            _world?.Destroy();
            _world = null;
            _worldDisposable = null;
            _systems = null;
            _fixedSystems = null;
            _lateSystems = null;
        }
    }
}