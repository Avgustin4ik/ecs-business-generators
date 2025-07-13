namespace GeneratorGame.Code.Ecs.Ui.Mono
{
    using Cysharp.Threading.Tasks;
    using GeneratorGame.Code.Services;
    using R3;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class UIGeneratorView : UIBase
    {
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI incomeText;
        [SerializeField] private Button levelUpButton;
        [SerializeField] private CanvasGroup upgradePanel;
        [SerializeField] private Image progressBar;

        public ReactiveProperty<bool> LevelUpClicked = new ReactiveProperty<bool>();

        private void Awake()
        {
            levelUpButton.OnClickAsObservable().Subscribe(_ => LevelUpClicked.Value = true).AddTo(this);
        }

        public async UniTaskVoid LoadUpgradePanelAsync(Upgrade[] upgrades)
        {
            upgradePanel.alpha = 0f;
            upgradePanel.interactable = false;
            upgradePanel.blocksRaycasts = false;

            throw new System.NotImplementedException("Implement the upgrade panel loading logic here.");
            foreach (var upgrade in upgrades)
            {
            }
        }

        public void SetLevelText(string text)
        {
            levelText.text = text;
        }

        public void SetIncomeText(string text)
        {
            incomeText.text = text;
        }

        public void UpdateProgress(float progress)
        {
            if (progressBar == null)
            {
                return;
            }

            progressBar.fillAmount = progress;
        }
    }
}