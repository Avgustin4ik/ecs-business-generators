namespace GeneratorGame.Code.Ecs.Ui.Systems
{
    using Leopotam.EcsLite;

    public class InitializeUiSystem : IEcsInitSystem
    {
        private EcsWorld _world;
        public void Init(IEcsSystems systems)
        {
            
            _world = systems.GetWorld();
            
            var uiCatalog = systems.GetShared<UiCatalog>();
            foreach (var ui in uiCatalog.uiPrefabs)
            {
                // ui.ApplyEcsWorld(_world);
            }
        }
    }
}