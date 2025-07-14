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
            
            levelUpButton.OnClickAsObservable().Subscribe(_ => Model.OnLevelUpSignal.SetValue(true)).AddTo(this);
            
            Model.levelUpAvailable.Subscribe(x => levelUpButton.interactable = x).AddTo(this);
            Model.levelupPrice.Subscribe(SetLevelUpPriceLabel).AddTo(this);
            Model.level.Subscribe(x => levelText.text = x.ToString());
            Model.income.Subscribe(x => incomeText.text = x.ToString());
            Model.spawn.Subscribe(SpawnButton).AddTo(this);

        }

        private void SetLevelUpPriceLabel(float price)
        {
            levelUpPriceLabel.text = $"Цена: {price}$";
        }

        private void SpawnButton(string upgradeGuid)
        {
            var tAsync = Object.InstantiateAsync<UpgradeButtonView>(upgradeButtonView, upgradePanel.transform).GetOperation();
            tAsync.WaitForCompletion();
            var button = (UpgradeButtonView)tAsync.Result.First();
            button.ApplyEcsWorld(EcsCore.EcsGlobalData.World);
            button.Model.Guid = upgradeGuid;
            Debug.Log($"Upgrade Button with guid {upgradeGuid} should be spawned");
        }

        public UpgradeButtonView upgradeButtonView;
        private void SpawnButton(ICollection<UpgradeButtonModel> upgrades)
        {
            if(upgrades == null) return;
            foreach (var upgradeButtonModel in upgrades)
            {
                var tAsync = Object.InstantiateAsync<UpgradeButtonView>(upgradeButtonView, upgradePanel.transform).GetOperation();
                tAsync.WaitForCompletion();
                var button = (UpgradeButtonView)tAsync.Result.First();
                button.ApplyEcsWorld(EcsCore.EcsGlobalData.World);
                var buttonModel = button.Model;
                buttonModel.Price.Value = upgradeButtonModel.Price.Value;
                buttonModel.Multiplayer.Value = upgradeButtonModel.Multiplayer.Value;
                buttonModel.IsInteractable.Value = upgradeButtonModel.IsInteractable.Value;
                buttonModel.Name.Value = upgradeButtonModel.Name.Value;
                buttonModel.IsPurchased.Value = upgradeButtonModel.IsPurchased.Value;
                
                // buttonModel.OnClick.Subscribe();
                // upgradeButtons.Add(buttonModel);
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
        public SignalValueProperty<bool> OnLevelUpSignal = new();
        
        public ReactiveProperty<string> label = new();
        public ReactiveProperty<float> levelupPrice = new();
        public ReactiveProperty<bool> levelUpAvailable = new ReactiveProperty<bool>();
        public ReactiveProperty<int> level = new();
        public ReactiveProperty<float> income = new();
        
        public ReactiveProperty<bool> purchaseAvailable = new ReactiveProperty<bool>();
        public ReactiveCommand<string> spawn = new();
    }
}