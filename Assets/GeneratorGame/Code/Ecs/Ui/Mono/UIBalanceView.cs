namespace GeneratorGame.Code.Ecs.Ui.Mono
{
    using System;
    using System.Text;
    using Leopotam.EcsLite;
    using TMPro;
    using UnityEngine;

    public class UIBalanceView : UIBase
    {
        [SerializeField] private TextMeshProUGUI balanceText;
        private readonly StringBuilder _stringBuilder = new StringBuilder(5);
        private const string StringFormat = "F1";

        public override void ApplyEcsWorld<T>(EcsWorld world, int entity = -1)
        {
            base.ApplyEcsWorld<UIBalanceView>(world, entity);
        }

        public void SetBalanceText(float text)
        {
            if (balanceText != null)
                balanceText.text = _stringBuilder
                    .Clear()
                    .Append("Balance: ")
                    .Append(text.ToString(StringFormat))
                    .Append(" $")
                    .ToString();
            else
            {
                Debug.LogError("Balance Text is not assigned in UIBalanceView.");
            }
        }
    }
}