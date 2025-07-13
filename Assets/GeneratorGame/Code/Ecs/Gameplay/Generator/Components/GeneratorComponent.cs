namespace GeneratorGame.Code.Ecs.Gameplay.Generator
{
    using System;
    using System.Collections.Generic;

    public struct GeneratorComponent
    {
        public int Level;
        public float BaseIncome;
        public float DurationInSeconds;
        public float UpgradesMultiplier;
        public DateTime NextIncomeTime;
        public string Guid;
    }
}