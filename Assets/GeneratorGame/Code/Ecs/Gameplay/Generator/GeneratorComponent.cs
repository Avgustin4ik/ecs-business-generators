namespace GeneratorGame.Code.Ecs.Gameplay.Generator
{
    using System;

    public struct GeneratorComponent
    {
        public int Level;
        public float Income;
        public float DurationInSeconds;
        public DateTime NextIncomeTime;
    }
}