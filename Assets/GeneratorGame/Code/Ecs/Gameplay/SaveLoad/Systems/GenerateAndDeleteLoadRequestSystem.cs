namespace GeneratorGame.Code.Ecs.Gameplay.SaveLoad.Systems
{
    using Leopotam.EcsLite;

    public class GenerateAndDeleteLoadRequestSystem : IEcsPostRunSystem, IEcsInitSystem
    {
        private readonly SaveLoadAspect _saveLoadAspect;
        private EcsWorld _world;

        public GenerateAndDeleteLoadRequestSystem(SaveLoadAspect saveLoadAspect)
        {
            _saveLoadAspect = saveLoadAspect;
        }
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _saveLoadAspect.LoadData.Add(_world.NewEntity());
        }
        public void PostRun(IEcsSystems systems)
        {
            foreach (var request in _saveLoadAspect.LoadDataRequestFilter)
            {
                _saveLoadAspect.LoadData.Del(request);
                return;
            }
        }
    }
}