namespace GeneratorGame.Code.Ecs.Gameplay.Generator.Systems
{
    using Leopotam.EcsLite;

    public class StartGameSystem : IEcsInitSystem
    {
        private readonly GeneratorAspect _aspect;

        public StartGameSystem(GeneratorAspect aspect)
        {
            _aspect = aspect;
        }

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _aspect.Balance.Add(world.NewEntity());
        }
    }
}