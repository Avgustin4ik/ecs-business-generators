namespace GeneratorGame.Code.Services
{
    using System;
    using UnityEngine;

    [CreateAssetMenu (fileName = "GeneratorDataServiceSource", menuName = "GeneratorGame/Services/GeneratorDataServiceSource")]
    public class GeneratorDataServiceSource : ScriptableObject, IServiceSource<GeneratorDataService>
    {
        public GeneratorData[] GeneratorDataList = Array.Empty<GeneratorData>();
        public GeneratorDataService CreateService()
        {
            return new GeneratorDataService(GeneratorDataList);
        }
    }
}