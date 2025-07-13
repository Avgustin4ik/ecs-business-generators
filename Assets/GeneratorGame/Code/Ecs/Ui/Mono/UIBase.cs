namespace GeneratorGame.Code.Ecs.Ui.Mono
{
    using System;
    using Gameplay;
    using Leopotam.EcsLite;
    using UnityEngine;

    public abstract class UIBase<TModel> : MonoBehaviour where TModel : Model, new()
    {
        private EcsWorld _world;
        public EcsPackedEntity Entity { get; private set; }
        public TModel Model { get; private set; }
        private bool _isInitialized = false;

        public virtual void ApplyEcsWorld(EcsWorld world, int entity = -1)
        {
            _world = world;
            var uiEntity = entity == -1 ? _world.NewEntity() : entity;
            ref var viewComponent = ref _world.GetPool<UIViewComponent<TModel>>().Add(uiEntity);
            // viewComponent.View = this;
            viewComponent.Type = GetType();
            Entity = _world.PackEntity(uiEntity);
            Model = new();
            viewComponent.Model = Model;
            OnInitialize();
            _isInitialized = true;
        }

        public virtual void OnInitialize()
        {
        }

        private void Start()
        {
            if (!_isInitialized)
            {
                _world = EcsCore.EcsGlobalData.World;
                if (_world == null)
                {
                    throw new InvalidOperationException("EcsWorld is not set. Please call ApplyEcsWorld before using this UIBase.");
                }
                ApplyEcsWorld(_world);
            }
        }

        protected virtual void OnDestroy()
        {
            _world = null;
            Entity = default;
        }
    }
}