namespace GeneratorGame.Code.Ecs.Ui.Mono
{
    using System;
    using Gameplay;
    using Leopotam.EcsLite;
    using UnityEngine;

    public abstract class UIBase : MonoBehaviour
    {
        EcsWorld _world;
        private EcsPackedEntity _entity;
        public EcsPackedEntity Entity => _entity;
        private bool _isInitialized = false;
        public virtual void ApplyEcsWorld<T>(EcsWorld world, int entity = -1)  where T : UIBase
        {
            
            _world = world;
            var uiEntity = entity == -1 ? _world.NewEntity() : entity;
            ref var viewComponent = ref _world.GetPool<UIViewComponent<T>>().Add(uiEntity);
            viewComponent.View = this;
            viewComponent.Type = GetType();
            _entity = _world.PackEntity(uiEntity);
            _isInitialized = true;
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
                ApplyEcsWorld<UIBase>(_world);
            }
        }

        protected virtual void OnDestroy()
        {
            _world = null;
            _entity = default;
        }
    }
}