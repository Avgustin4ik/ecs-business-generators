namespace GeneratorGame.Code.Ecs.Ui.Mono
{
    using Cysharp.Threading.Tasks;
    using Leopotam.EcsLite;
    using R3;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class PurchaseButtonView : UIBase<PurchaseButtonModel>
    {
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI priceLabel;
        [SerializeField] private TextMeshProUGUI rewardLabel;

        public override void OnInitialize()
        {
            base.OnInitialize();
            Model.Reward.Subscribe(UpdateRewardLabel).AddTo(this);
            Model.Price.Subscribe(UpdatePriceLabel).AddTo(this);
            Model.Active.Subscribe(SetButtonActive).AddTo(this);
            Model.Purchased.Subscribe(Purchased).AddTo(this);
            Model.OnClick = button.OnClickAsObservable();
        }

        private void UpdateRewardLabel(float valueInPercentage)
        {
            if (rewardLabel != null) rewardLabel.text = $"Доход: + {valueInPercentage}%";
        }
        private void Purchased(bool value)
        {
            if(!value) return;
            if (priceLabel != null) priceLabel.text = "Куплено!";
            Model.Active.Value = false;
        }

        private void SetButtonActive(bool value)
        {
            if (button) button.interactable = value;
        }
        private void UpdatePriceLabel(float value)
        {
            if (priceLabel) priceLabel.text = value.ToString();
        }
    }

    public class PurchaseButtonModel : Model
    {
        public ReactiveProperty<float> Price = new();
        public ReactiveProperty<float> Reward = new();
        public ReactiveProperty<bool> Active = new();
        public ReactiveProperty<bool> Purchased = new();
        public Observable<Unit> OnClick = new Subject<Unit>();
    }
}