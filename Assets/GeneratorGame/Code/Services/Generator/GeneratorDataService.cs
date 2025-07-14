namespace GeneratorGame.Code.Services.Generator
{
    using System.Collections.Generic;
    using System.Linq;

    public class GeneratorDataService : IGeneratorDataService
    {
        private readonly Dictionary<string, GeneratorData> _generatorDataDictionary;
        public GeneratorDataService(GeneratorData[] generatorDataList)
        {
            _generatorDataDictionary = new Dictionary<string, GeneratorData>(5);
            foreach (var data in generatorDataList)
            {
                _generatorDataDictionary.TryAdd(data.Guid, data);
            }
        }

        public GeneratorData GetGeneratorData(string guid) => _generatorDataDictionary[guid];

        public float GetBaseIncome(string generatorGuid) => _generatorDataDictionary[generatorGuid].Income;

        public float[] GetUpgradesMultiplier(string generatorGuid)
        {
            return _generatorDataDictionary[generatorGuid].Upgrades.Select(x => x.IncomeMultiplier).ToArray();
        }

        public Upgrade[] GetUpgrades(string generatorGuid)
        {
            return _generatorDataDictionary[generatorGuid].Upgrades;
        }

        public IEnumerable<GeneratorData> GetAllGenerators()
        {
            return _generatorDataDictionary.Values;
        }
    }
}