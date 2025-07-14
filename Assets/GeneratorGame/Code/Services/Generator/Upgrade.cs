namespace GeneratorGame.Code.Services.Generator
{
    using System;

    [Serializable]
    public class Upgrade
    {
        public Upgrade()
        {
            guid = System.Guid.NewGuid().ToString();
        }
        public float Price;
        public float IncomeMultiplier;
        public string Name;
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
    }
}