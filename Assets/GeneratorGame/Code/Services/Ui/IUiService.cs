namespace GeneratorGame.Code.Services.Ui
{
    using Ecs.Ui.Mono;
    using UnityEngine;

    public interface IUiService
    {
        UIGeneratorView GeneratorPrefab { get; }
    }

    public class UiService : IUiService
    {
        private readonly UIGeneratorView _uiGeneratorViewPrefab;
        private readonly Transform _uiRoot;
        public UiService(UIGeneratorView uiGeneratorViewPrefab, Transform uiRoot)
        {
            _uiGeneratorViewPrefab = uiGeneratorViewPrefab;
            _uiRoot = uiRoot;
        }

        public UIGeneratorView GeneratorPrefab => _uiGeneratorViewPrefab;
    }
}