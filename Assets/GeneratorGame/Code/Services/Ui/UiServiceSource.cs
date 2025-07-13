namespace GeneratorGame.Code.Services.Ui
{
    using Ecs.Ui;
    using Ecs.Ui.Mono;
    using UnityEngine;

    [CreateAssetMenu(fileName = "UiServiceSource", menuName = "GeneratorGame/Services/UiServiceSource")]
    public class UiServiceSource : ScriptableObject, IServiceSource<IUiService>
    {
        [SerializeField] private UIGeneratorView uiGeneratorViewPrefab;
        [SerializeField] private Transform uiRoot;       
        public IUiService CreateService()
        {
            return new UiService(uiGeneratorViewPrefab, uiRoot);
        }
    }
}