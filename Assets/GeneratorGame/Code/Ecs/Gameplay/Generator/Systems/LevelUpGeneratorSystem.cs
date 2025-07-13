namespace GeneratorGame.Code.Ecs.Gameplay.Generator
{
    using Leopotam.EcsLite;
    using Services;

    public class LevelUpGeneratorSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly GeneratorAspect _aspect;
        private EcsWorld _world;
        private EcsFilter _filter;
        private IGeneratorDataService _service;

        public LevelUpGeneratorSystem(GeneratorAspect aspect)
        {
            _aspect = aspect;
        }

        public void Init(IEcsSystems systems)
        {
            _filter = systems.GetWorld().Filter<LevelUpGeneratorSelfEvent>().Inc<GeneratorComponent>().End();
            _service = systems.GetShared<IGeneratorDataService>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var generator = ref _aspect.Generator.Get(entity);
                generator.Level++;
            }
        }
    }
}