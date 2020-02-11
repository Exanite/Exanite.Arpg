using UniRx.Async;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Exanite.Arpg.AssetRegistry.Providers
{
    public class ResourceAssetProvider<T> : AssetProvider<T> where T : Object
    {
        private readonly string key;

        public string Key
        {
            get
            {
                return key;
            }
        }

        public ResourceAssetProvider(string key)
        {
            this.key = key;
        }

        protected override async UniTask<object> LoadInnerAsync()
        {
            return await Resources.LoadAsync(key).ToUniTask();
        }

        public override void Unload()
        {
            Resources.UnloadAsset(TypedAsset);
            Asset = null;
        }
    }
}
