namespace GeneratorGame.Code.Ecs.Ui.Mono
{
    using Cysharp.Threading.Tasks;
    using GeneratorGame.Code.Services;
    using R3;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class UIGeneratorView : UIBase<UIGeneratorModel>
    {
        #region inspector

        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI incomeText;
        [SerializeField] private Button levelUpButton;
        [SerializeField] private CanvasGroup upgradePanel;
        [SerializeField] private Image progressBar;
        
        #endregion
        
        public override void OnInitialize()
        {
            base.OnInitialize();
            Model.progress.Subscribe(UpdateProgress).AddTo(this);
            levelUpButton.OnClickAsObservable().Subscribe(_ => Model.levelup = true).AddTo(this);
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

    public class UIGeneratorModel : Model
    {
        public ReactiveProperty<float> progress = new();
        public bool levelup = false;
    }
}