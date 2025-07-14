namespace GeneratorGame.Code.Ecs.Ui.Mono
{
    using System.Collections.Generic;
    using System.Linq;
    using Gameplay;
    using R3;
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
        public UpgradeButtonView upgradeButtonPrefab;
 
        #endregion
        
        public override void OnInitialize()
        {
            base.OnInitialize();
            Model.progress.Subscribe(UpdateProgress).AddTo(this);
            
            levelUpButton.OnClickAsObservable().Subscribe(_ => Model.OnLevelUpSignal.SetValue(true)).AddTo(this);
            
            Model.levelUpAvailable.Subscribe(x => levelUpButton.interactable = x).AddTo(this);
            Model.levelupPrice.Subscribe(SetLevelUpPriceLabel).AddTo(this);
            Model.level.Subscribe(SetLevelText);
            Model.income.Subscribe(SetIncomeText);
            Model.spawn.Subscribe(SpawnButton).AddTo(this);

        }

        private void SetLevelUpPriceLabel(float price)
        {
            levelUpPriceLabel.text = $"Цена: {price}$";
        }

        private void SpawnButton(string upgradeGuid)
        {
            var tAsync = Object.InstantiateAsync<UpgradeButtonView>(upgradeButtonPrefab, upgradePanel.transform).GetOperation();
            tAsync.WaitForCompletion();
            var button = (UpgradeButtonView)tAsync.Result.First();
            button.ApplyEcsWorld(EcsCore.EcsGlobalData.World);
            button.Model.Guid = upgradeGuid;
            Debug.Log($"Upgrade Button with guid {upgradeGuid} should be spawned");
        }


        private void SetLevelText(int value) => levelText.text = value.ToString();

        private void SetIncomeText(float value) => incomeText.text = value.ToString();

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