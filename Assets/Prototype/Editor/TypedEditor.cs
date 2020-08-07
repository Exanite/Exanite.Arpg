using UnityEngine;

namespace Prototype.Editor
{
    public abstract class TypedEditor<T> : UnityEditor.Editor where T : Object
    {
        public T TypedTarget;

        protected virtual void OnEnable()
        {
            TypedTarget = (T)target;
        }
    }
}
