namespace GeneratorGame.Code.Ecs
{
    using Leopotam.EcsLite;
    using UnityEngine;

    public class ApplicationQuitHandler : MonoBehaviour
    {
        private EcsWorld _world;
        private EcsSystems _systems;
        private bool _hasPaused;

        public void Initialize(EcsWorld world, EcsSystems systems)
        {
            _world = world;
            _systems = systems;
            _hasPaused = false;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus && !_hasPaused)
            {
                _systems?.Run();
                _hasPaused = true;
            }
            else if (hasFocus)
            {
                _hasPaused = false;
            }
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus && !_hasPaused)
            {
                _systems?.Run();
                _hasPaused = true;
            }
            else if (!pauseStatus)
            {
                _hasPaused = false;
            }
        }

        private void OnApplicationQuit()
        {
            _systems?.Run();
        }
    }

}