namespace GeneratorGame.Code.Ecs.Ui.Mono
{
    using System.Text;
    using R3;
    using TMPro;
    using UnityEngine;

    public class UIBalanceView : UIBase<UIBalanceModel>
    {
        [SerializeField] private TextMeshProUGUI balanceText;
        private readonly StringBuilder _stringBuilder = new StringBuilder(5);
        private const string StringFormat = "F1";

        public override void OnInitialize()
        {
            base.OnInitialize();
            Model.Balance.Subscribe(SetBalanceText).AddTo(this);
        }

        public void SetBalanceText(float value)
        {
            if (balanceText != null)
                balanceText.text = _stringBuilder
                    .Clear()
                    .Append("Balance: ")
                    .Append(value.ToString(StringFormat))
                    .Append(" $")
                    .ToString();
            else
            {
                Debug.LogError("Balance Text is not assigned in UIBalanceView.");
            }
        }
    }

    public class UIBalanceModel : Model
    {
        public readonly ReactiveProperty<float> Balance = new ReactiveProperty<float>();
    }
}