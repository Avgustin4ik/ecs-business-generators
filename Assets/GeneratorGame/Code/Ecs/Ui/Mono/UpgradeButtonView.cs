namespace GeneratorGame.Code.Ecs.Ui.Mono
{
    using System.Text;
    using R3;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class UpgradeButtonView : UIBase<UpgradeButtonModel>
    {
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI priceLabel;
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private TextMeshProUGUI description;
        private readonly StringBuilder _cachedString = new StringBuilder(20);
        public override void OnInitialize()
        {
            base.OnInitialize();

            Model.Name.Subscribe(x => label.text = x).AddTo(this);
            Model.Multiplayer.Subscribe(UpdateDescription).AddTo(this);
            Model.Price.Subscribe(UpdatePrice).AddTo(this);
            Model.IsPurchased.Subscribe(SetPurchased).AddTo(this);
            Model.IsInteractable.Subscribe(SetInteractable).AddTo(this);
            button.OnClickAsObservable().Subscribe(_ => Model.OnUpgradeSignal.SetValue(Model.Guid)).AddTo(this);
        }

        private void UpdateDescription(float x)
        {
            if (x < 1) x *= 100f;
            _cachedString.Clear();
            _cachedString
                .Append("Доход: + ")
                .Append(x)
                .Append("%");
            description.text = _cachedString.ToString();
        }

        private void SetInteractable(bool value)
        {
            button.interactable = value;
        }

        private void SetPurchased(bool value)
        {
            if (value) priceLabel.text = "Куплено!";
            Model.IsInteractable.Value = false;
        }

        private void UpdatePrice(float value)
        {
            if (Model.IsPurchased.Value) return;
            _cachedString.Clear();
            _cachedString
                .Append("Цена: ")
                .Append(value)
                .Append("$");
            priceLabel.text = value <= 0 ? "Бесплатно!" : _cachedString.ToString();
        }
    }

    public class UpgradeButtonModel : Model
    {
        public ReactiveProperty<string> Name = new();
        public ReactiveProperty<float> Multiplayer = new();
        public ReactiveProperty<float> Price = new();
        public ReactiveProperty<bool> IsPurchased = new();
        public ReactiveProperty<bool> IsInteractable = new();
        public string Guid;
        public SignalValueProperty<string> OnUpgradeSignal = new();
    }
}