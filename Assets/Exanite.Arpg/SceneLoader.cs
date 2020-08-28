using System;
using UniRx.Async;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Exanite.Arpg
{
    /// <summary>
    /// Used to load scenes<para/>
    /// Note: Only use this class to load scenes, directly loading with <see cref="SceneManager"/> will break the Extenject bindings and container parenting
    /// </summary>
    public class SceneLoader
    {
        private bool isLoading;

        private SceneContextRegistry sceneContextRegistry;

        [Inject]
        public void Inject(SceneContextRegistry sceneContextRegistry)
        {
            this.sceneContextRegistry = sceneContextRegistry;
        }

        /// <summary>
        /// Loads the <see cref="Scene"/> using the provided <see cref="Scene"/> as its parent
        /// </summary>
        /// <param name="sceneName">The name of the <see cref="Scene"/> to load</param>
        /// <param name="parent">The parent of the new <see cref="Scene"/></param>
        /// <param name="bindings">Bindings to install to the <see cref="DiContainer"/></param>
        /// <param name="bindingsLate">Late bindings to install to the <see cref="DiContainer"/>, these are installed after all other bindings are installed</param>
        /// <returns>The newly loaded <see cref="Scene"/></returns>
        public UniTask<Scene> LoadAdditiveSceneAsync(string sceneName, Scene parent, Action<DiContainer> bindings = null, Action<DiContainer> bindingsLate = null)
        {
            var context = sceneContextRegistry.TryGetSceneContextForScene(parent);

            return LoadAdditiveSceneAsync(sceneName, context, bindings, bindingsLate);
        }

        /// <summary>
        /// Loads the <see cref="Scene"/> using the provided <see cref="SceneContext"/> as its parent
        /// </summary>
        /// <param name="sceneName">The name of the <see cref="Scene"/> to load</param>
        /// <param name="parent">The parent of the new <see cref="Scene"/></param>
        /// <param name="bindings">Bindings to install to the <see cref="DiContainer"/></param>
        /// <param name="bindingsLate">Late bindings to install to the <see cref="DiContainer"/>, these are installed after all other bindings are installed</param>
        /// <returns>The newly loaded <see cref="Scene"/></returns>
        public async UniTask<Scene> LoadAdditiveSceneAsync(string sceneName, SceneContext parent, Action<DiContainer> bindings = null, Action<DiContainer> bindingsLate = null)
        {
            if (parent == null)
            {
                throw new ArgumentNullException(nameof(parent));
            }

            if (!Application.CanStreamedLevelBeLoaded(sceneName))
            {
                throw new ArgumentException($"Failed to load scene. Specified scene '{sceneName}' does not exist", nameof(sceneName));
            }

            // Allow only one scene to load at a time
            await UniTask.WaitWhile(() => isLoading); 
            isLoading = true;

            PrepareForSceneLoad(parent, bindings, bindingsLate);

            try
            {
                var loadSceneParameters = new LoadSceneParameters(LoadSceneMode.Additive, LocalPhysicsMode.Physics3D);
                await SceneManager.LoadSceneAsync(sceneName, loadSceneParameters);

                // LoadSceneAsync does not return the newly loaded scene, this is the only way to get the new scene
                Scene scene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);

                // Wait for scene to initialize
                await UniTask.Yield(); 

                return scene;
            }
            finally
            {
                // Prevent dead lock
                isLoading = false;
            }
        }

        /// <summary>
        /// Prepares the next scene loaded to use the provided parent and bindings
        /// </summary>
        private void PrepareForSceneLoad(SceneContext parent, Action<DiContainer> bindings, Action<DiContainer> bindingsLate)
        {
            var sceneContainer = parent.Container;

            SceneContext.ParentContainers = new[] { sceneContainer };

            SceneContext.ExtraBindingsInstallMethod = bindings;
            SceneContext.ExtraBindingsLateInstallMethod = bindingsLate;
        }
    }
}
