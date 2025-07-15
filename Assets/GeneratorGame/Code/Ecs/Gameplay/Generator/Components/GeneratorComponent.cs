namespace GeneratorGame.Code.Ecs.Gameplay.Generator.Components
{
    public struct GeneratorComponent
    {
        public int Level;
        public float BaseIncome;
        public float DurationInSeconds;
        public float UpgradesMultiplier;
        public float Progress;
        public string Guid;
        public float CurrentIncome;
        public string Name;

        public void UpdateIncome() => CurrentIncome = Level * BaseIncome * (1f + UpgradesMultiplier);
    }
}