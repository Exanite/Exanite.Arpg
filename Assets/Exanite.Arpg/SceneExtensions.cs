using UnityEngine;
using UnityEngine.SceneManagement;

namespace Exanite.Arpg
{
    public static class SceneExtensions
    {
        public static GameObject Instantiate(this Scene scene, GameObject original, Vector3 position, Quaternion rotation)
        {
            var gameObject = Object.Instantiate(original, position, rotation);
            SceneManager.MoveGameObjectToScene(gameObject, scene);

            return gameObject;
        }

        public static T Instantiate<T>(this Scene scene, T original, Vector3 position, Quaternion rotation) where T : Component
        {
            var component = Object.Instantiate(original, position, rotation);
            SceneManager.MoveGameObjectToScene(component.gameObject, scene);

            return component;
        }

        public static GameObject InstantiateNew(this Scene scene, string name)
        {
            var gameObject = new GameObject(name);
            SceneManager.MoveGameObjectToScene(gameObject, scene);

            return gameObject;
        }
    }
}
