using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Exanite.Arpg.Installers
{
    /// <summary>
    /// Installs the <see cref="Scene"/> and <see cref="PhysicsScene"/> this component is in to a <see cref="DiContainer"/>
    /// </summary>
    public class PhysicsSceneInstaller : MonoInstaller
    {
        [SerializeField] private bool requireLocalPhysicsScene = true;

        /// <summary>
        /// Should this <see cref="PhysicsSceneInstaller"/> require that this scene is loaded with a local <see cref="PhysicsScene"/>
        /// </summary>
        public bool RequireLocalPhysicsScene
        {
            get
            {
                return requireLocalPhysicsScene;
            }

            set
            {
                requireLocalPhysicsScene = value;
            }
        }

        /// <summary>
        /// Installs bindings to the <see cref="DiContainer"/>
        /// </summary>
        public override void InstallBindings()
        {
            Container.Bind<Scene>().FromMethod(GetScene).AsSingle().NonLazy();
            Container.Bind<PhysicsScene>().FromMethod(GetPhysicsScene).AsSingle().NonLazy();
        }

        /// <summary>
        /// Gets the <see cref="Scene"/> this component is currently in
        /// </summary>
        private Scene GetScene()
        {
            return gameObject.scene;
        }

        /// <summary>
        /// Gets the <see cref="PhysicsScene"/> this component is currently in
        /// </summary>
        private PhysicsScene GetPhysicsScene()
        {
            var physicsScene = gameObject.scene.GetPhysicsScene();

            if (RequireLocalPhysicsScene && physicsScene == Physics.defaultPhysicsScene)
            {
                throw new InvalidOperationException($"Scene PhysicsScene is same as global. Make sure this scene is not loaded with the option 'LocalPhysicsMode.None'.");
            }

            return physicsScene;
        }
    }
}
