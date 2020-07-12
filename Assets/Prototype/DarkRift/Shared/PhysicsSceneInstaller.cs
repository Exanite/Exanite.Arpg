using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Prototype.DarkRift.Shared
{
    public class PhysicsSceneInstaller : MonoInstaller
    {
        [SerializeField] private bool requireLocalPhysicsScene = true;

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

        public override void InstallBindings()
        {
            Container.Bind<Scene>().FromMethod(GetScene).AsSingle().NonLazy();
            Container.Bind<PhysicsScene>().FromMethod(GetPhysicsScene).AsSingle().NonLazy();
        }

        private Scene GetScene()
        {
            return gameObject.scene;
        }

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
