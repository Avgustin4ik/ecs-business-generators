namespace GeneratorGame.Code.Ecs.Gameplay.Generator.Systems
{
    using Components;
    using Leopotam.EcsLite;

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
            _filter = systems.GetWorld().Filter<LevelUpGeneratorSelfRequest>().Inc<GeneratorComponent>().Inc<LevelUpPriceComponent>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var generator = ref _aspect.Generator.Get(entity);
                generator.Level++;
                generator.UpdateIncome();
                ref var price = ref _aspect.LevelUpPrice.Get(entity);
                price.UpdatePrice(generator.Level);
                _aspect.LevelUp.Del(entity);
            }
        }
    }
}