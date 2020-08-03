using System;
using Exanite.Arpg;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Prototype.LiteNetLib.Levels
{
    public class Level
    {
        public Guid guid;
        public Transform root;

        private Scene scene;

        public Level(GameObject levelPrefab)
        {
            guid = Guid.NewGuid();

            var createSceneParameters = new CreateSceneParameters(LocalPhysicsMode.Physics3D);
            scene = SceneManager.CreateScene(guid.ToString(), createSceneParameters);

            root = CreateLevel(levelPrefab);
        }

        private Transform CreateLevel(GameObject levelPrefab)
        {
            var levelGameObject = scene.Instantiate(levelPrefab, Vector3.zero, Quaternion.identity);

            return levelGameObject.transform;
        }
    }
}
