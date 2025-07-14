namespace GeneratorGame.Code.Ecs.Ui.Mono
{
    using System;
    using R3;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class UpgradeButtonView : UIBase<UpgradeButtonModel>
    {
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI priceLabel;
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private TextMeshProUGUI descriprion;
        
        public override void OnInitialize()
        {
            base.OnInitialize();
            Model.Name.Subscribe(x => label.text = x).AddTo(this);
            Model.Multiplayer.Subscribe(x => descriprion.text = $"Доход: + {x}%").AddTo(this);
            Model.Price.Subscribe(UpdatePrice).AddTo(this);
            Model.IsPurchased.Subscribe(SetPurchased).AddTo(this);
            Model.IsInteractable.Subscribe(SetInteractable).AddTo(this);
            button.OnClickAsObservable().Subscribe(_ => Model.Click.OnNext(Model.Guid)).AddTo(this);
            button.OnClickAsObservable().Subscribe(_ => Model.OnUpgradeSignal.SetValue(Model.Guid));
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
            if(Model.IsPurchased.Value) return;
            label.text = value <= 0 ? "Бесплатно!" : $"Цена: {value}$";
        }
    }

    public class UpgradeButtonModel : Model
    {
        public Subject<string> Click = new();
        public ReactiveProperty<string> Name = new();
        public ReactiveProperty<float> Multiplayer = new();
        public ReactiveProperty<float> Price = new();
        public ReactiveProperty<bool> IsPurchased = new();
        public ReactiveProperty<bool> IsInteractable = new();
        public string Guid;
        public SignalValueProperty<string> OnUpgradeSignal = new();
    }
}