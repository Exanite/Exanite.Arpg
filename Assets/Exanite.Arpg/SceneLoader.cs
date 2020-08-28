using System;
using UniRx.Async;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Exanite.Arpg
{
    public class SceneLoader
    {
        private bool isLoading;

        private SceneContextRegistry sceneContextRegistry;

        [Inject]
        public void Inject(SceneContextRegistry sceneContextRegistry)
        {
            this.sceneContextRegistry = sceneContextRegistry;
        }

        public UniTask LoadAdditiveSceneAsync(string sceneName, Scene parent, Action<DiContainer> bindings = null, Action<DiContainer> bindingsLate = null)
        {
            var context = sceneContextRegistry.TryGetSceneContextForScene(parent);

            return LoadAdditiveSceneAsync(sceneName, context, bindings, bindingsLate);
        }

        public async UniTask LoadAdditiveSceneAsync(string sceneName, SceneContext parent, Action<DiContainer> bindings = null, Action<DiContainer> bindingsLate = null)
        {
            if (parent == null)
            {
                throw new ArgumentNullException(nameof(parent));
            }

            if (!Application.CanStreamedLevelBeLoaded(sceneName))
            {
                throw new ArgumentException($"Failed to load scene. Specified scene '{sceneName}' does not exist", nameof(sceneName));
            }

            await UniTask.WaitWhile(() => isLoading);
            isLoading = true;

            PrepareForSceneLoad(parent, bindings, bindingsLate);

            try
            {
                var loadSceneParameters = new LoadSceneParameters(LoadSceneMode.Additive, LocalPhysicsMode.Physics3D);
                await SceneManager.LoadSceneAsync(sceneName, loadSceneParameters).ToUniTask();
            }
            finally
            {
                isLoading = false; // prevent dead lock
            }
        }

        private void PrepareForSceneLoad(SceneContext parent, Action<DiContainer> bindings, Action<DiContainer> bindingsLate)
        {
            var sceneContainer = parent.Container;

            SceneContext.ParentContainers = new[] { sceneContainer };

            SceneContext.ExtraBindingsInstallMethod = bindings;
            SceneContext.ExtraBindingsLateInstallMethod = bindingsLate;
        }
    }
}
