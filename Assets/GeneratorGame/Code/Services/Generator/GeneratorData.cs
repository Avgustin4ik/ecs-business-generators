namespace GeneratorGame.Code.Services.Generator
{
    using System;

    [Serializable]
    public class GeneratorData
    {
        public GeneratorData()
        {
            guid = System.Guid.NewGuid().ToString();
        }
        public float BasePrice;
        public string Name;
        public int Level = 0;
        public float Income = 1.0f;
        public float DurationInSeconds = 100000f;
        [UnityEngine.SerializeField]
        private string guid = null;
        public string Guid
        {
            get
            {
                if (string.IsNullOrEmpty(guid))
                    guid = System.Guid.NewGuid().ToString();
                return guid;
            }
            private set { guid = value; }
        }
        public Upgrade[] Upgrades = Array.Empty<Upgrade>();
    }
}