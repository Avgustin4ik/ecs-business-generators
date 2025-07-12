namespace GeneratorGame.Code.Services
{
    using System;
    using System.Collections.Generic;

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

        public float GetUpgradesMultiplier(string generatorGuid)
        {
            throw new NotImplementedException();
        }
    }
}