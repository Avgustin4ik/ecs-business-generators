namespace GeneratorGame.Code.Ecs.Gameplay.Generator
{
    public struct LevelUpPriceComponent
    {
        public float BasePrice;
        public float Next;

        public void UpdatePrice(int generatorLevel)
        {
            Next = (1 + generatorLevel) * BasePrice;
        }
    }
}