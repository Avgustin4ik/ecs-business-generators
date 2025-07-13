namespace GeneratorGame.Code.Services
{
    using System;

    [Serializable]
    public class GeneratorData
    {
        public string Guid { get; } = System.Guid.NewGuid().ToString();
        public float BasePrice;
        public string Name;
        public int Level = 0;
        public float Income = 1.0f;
        public float DurationInSeconds = 100000f;
        public Upgrade[] Upgrades = Array.Empty<Upgrade>();
    }
}