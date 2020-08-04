using System;
using Exanite.Arpg;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Prototype.LiteNetLib.Zones
{
    public class Zone
    {
        public Guid guid;
        public Transform root;

        private Scene scene;

        public Zone(GameObject zonePrefab)
        {
            guid = Guid.NewGuid();

            var createSceneParameters = new CreateSceneParameters(LocalPhysicsMode.Physics3D);
            scene = SceneManager.CreateScene(guid.ToString(), createSceneParameters);

            root = CreateZone(zonePrefab);
        }

        private Transform CreateZone(GameObject levelPrefab)
        {
            var levelGameObject = scene.Instantiate(levelPrefab, Vector3.zero, Quaternion.identity);

            return levelGameObject.transform;
        }
    }
}
