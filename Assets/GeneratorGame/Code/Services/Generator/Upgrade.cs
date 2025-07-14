namespace GeneratorGame.Code.Services.Generator
{
    using System;

    [Serializable]
    public class Upgrade
    {
        public float Price;
        public float IncomeMultiplier;
        public string Name;
        public string Guid { get; } = System.Guid.NewGuid().ToString();
    }
}