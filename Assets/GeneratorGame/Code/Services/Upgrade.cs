namespace GeneratorGame.Code.Services
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