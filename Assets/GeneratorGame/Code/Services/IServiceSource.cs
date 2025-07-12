namespace GeneratorGame.Code.Services
{
    public interface IServiceSource<out T>
    {
        public T CreateService();
    }
}