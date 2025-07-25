﻿namespace GeneratorGame.Code.Services.Generator
{
    using System.Collections.Generic;

    public interface IGeneratorDataService
    {
        // Define methods that the service should implement
        // For example, you might want to get generator data by GUID
        GeneratorData GetGeneratorData(string guid);
        float GetBaseIncome(string generatorGuid);
        float[] GetUpgradesMultiplier(string generatorGuid);
        Upgrade[] GetUpgrades(string generatorGuid);
        IEnumerable<GeneratorData> GetAllGenerators();
    }
}