namespace GeneratorGame.Code.Ecs.Gameplay.Generator
{
    using Leopotam.EcsLite;
    using Services;

    public class LevelUpGeneratorSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly GeneratorAspect _aspect;
        private EcsFilter _filter;

        public LevelUpGeneratorSystem(GeneratorAspect aspect)
        {
            _aspect = aspect;
        }

        public void Init(IEcsSystems systems)
        {
            _filter = systems.GetWorld().Filter<LevelUpGeneratorRequest>().Inc<GeneratorComponent>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var generator = ref _aspect.Generator.Get(entity);
                generator.Level++;
                _aspect.LevelUp.Del(entity);
            }
        }
    }
}