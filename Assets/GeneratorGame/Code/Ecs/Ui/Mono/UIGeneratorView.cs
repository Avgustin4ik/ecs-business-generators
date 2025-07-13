namespace GeneratorGame.Code.Ecs.Ui.Mono
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using Gameplay;
    using R3;
    using Services;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class UIGeneratorView : UIBase<UIGeneratorModel>
    {
        #region inspector
        
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI incomeText;
        [SerializeField] private TextMeshProUGUI levelUpPriceLabel;
        [SerializeField] private Button levelUpButton;
        [SerializeField] private CanvasGroup upgradePanel;
        [SerializeField] private Image progressBar;
 
        private List<PurchaseButtonModel> upgradeButtons = new List<PurchaseButtonModel>();
        
        #endregion
        
        public override void OnInitialize()
        {
            base.OnInitialize();
            Model.progress.Subscribe(UpdateProgress).AddTo(this);
            
            levelUpButton.OnClickAsObservable().Subscribe(_ => Model.levelup = true).AddTo(this);
            
            Model.levelUpAvailable.Subscribe(x => levelUpButton.interactable = x).AddTo(this);
            Model.levelupPrice.Subscribe(SetLevelUpPriceLabel).AddTo(this);
            Model.level.Subscribe(x => levelText.text = x.ToString());
            Model.income.Subscribe(x => incomeText.text = x.ToString());            
            
            Model.upgrades.Subscribe(SpawnButton).AddTo(this);
        }

        private void SetLevelUpPriceLabel(float price)
        {
            levelUpPriceLabel.text = $"Цена: {price}$";
        }

        public PurchaseButtonView upgradeButtonView;
        private void SpawnButton(ICollection<Upgrade> upgrades)
        {
            if(upgrades == null) return;
            foreach (var upgrade in upgrades)
            {
                var tAsync = Object.InstantiateAsync<PurchaseButtonView>(upgradeButtonView, upgradePanel.transform).GetOperation();
                tAsync.WaitForCompletion();
                var button = (PurchaseButtonView)tAsync.Result.First();
                button.ApplyEcsWorld(EcsCore.EcsGlobalData.World);
                button.Model.Price.Value = upgrade.Price;
                button.Model.Reward.Value = upgrade.IncomeMultiplier;
                button.Model.OnClick.Subscribe();
                upgradeButtons.Add(button.Model);
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(upgradePanel.transform as RectTransform);
        }

        private void SetLevelText(string text)
        {
            levelText.text = text;
        }

        private void SetIncomeText(string text)
        {
            incomeText.text = text;
        }

        private void UpdateProgress(float progress)
        {
            if (progressBar != null) progressBar.fillAmount = progress;
        }
    }

    public class UIGeneratorModel : Model
    {
        public ReactiveProperty<float> progress = new();
        public bool levelup = false;
        public float upgrade = default;
        
        public ReactiveProperty<string> label = new();
        public ReactiveProperty<float> levelupPrice = new();
        public ReactiveProperty<bool> levelUpAvailable = new ReactiveProperty<bool>();
        public ReactiveProperty<int> level = new();
        public ReactiveProperty<float> income = new();
        
        public ReactiveProperty<bool> purchaseAvailable = new ReactiveProperty<bool>();
        public ReactiveProperty<ICollection<Upgrade>> upgrades = new ReactiveProperty<ICollection<Upgrade>>(new List<Upgrade>());
    }
}