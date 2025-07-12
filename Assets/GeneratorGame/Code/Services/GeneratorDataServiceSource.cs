namespace GeneratorGame.Code.Services
{
    public class GeneratorDataServiceSource : IServiceSource<GeneratorDataService>
    {
        public GeneratorData[] GeneratorDataList;
        
        public GeneratorDataService CreateService()
        {
            return new GeneratorDataService(GeneratorDataList);
        }
    }
}