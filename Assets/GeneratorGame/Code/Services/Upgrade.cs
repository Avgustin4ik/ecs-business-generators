namespace GeneratorGame.Code.Services
{
    using System;

    [Serializable]
    public struct Upgrade
    {
        public string Name;
        public float Cost;
        public float IncomeMultiplier;
        public bool Purchased;
    }
}