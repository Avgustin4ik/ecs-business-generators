namespace GeneratorGame.Code.Ecs.Gameplay.Generator.Systems
{
    using System.Linq;
    using Services;
    using Leopotam.EcsLite;
    using Services.Generator;

    public class CreateGeneratorSystem : IEcsInitSystem
    {
        private readonly GeneratorAspect _aspect;
        private IGeneratorDataService _config;

        public CreateGeneratorSystem(GeneratorAspect aspect)
        {
            _aspect = aspect;
        }

        public void Init(IEcsSystems systems)
        {
            _config = systems.GetShared<IGeneratorDataService>();
            var world = systems.GetWorld();
            var index = 0;
            foreach (var data in _config.GetAllGenerators())
            {
                var entity = world.NewEntity();
                
                ref var generator = ref _aspect.Generator.Add(entity);
                generator.Guid = data.Guid;
                generator.Level = index == 0 ? 1 : 0;
                generator.BaseIncome = data.Income;
                generator.CurrentIncome = data.Income;
                generator.DurationInSeconds = data.DurationInSeconds;
                generator.UpgradesMultiplier = 0f;
                ref var levelUpPriceComponent = ref _aspect.LevelUpPrice.Add(entity);
                
                levelUpPriceComponent.BasePrice = data.BasePrice;
                levelUpPriceComponent.UpdatePrice(generator.Level);

                for (var i = 0; i < data.Upgrades.Length; i++)
                {
                    var availableUpgrade = data.Upgrades[i];
                    ref var upgrade = ref _aspect.AvailableUpgrade.Add(world.NewEntity());
                    upgrade.Multiplayer = availableUpgrade.IncomeMultiplier;
                    upgrade.Name = availableUpgrade.Name;
                    upgrade.Price = availableUpgrade.Price;
                    upgrade.Guid = availableUpgrade.Guid;
                    upgrade.GeneratorGuid = generator.Guid;
                }
                index++;
            }
        }
    }
}