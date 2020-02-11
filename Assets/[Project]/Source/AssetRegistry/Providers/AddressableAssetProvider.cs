//using UniRx.Async;
//using UnityEngine.AddressableAssets;
//using Object = UnityEngine.Object;

//namespace Exanite.Arpg.AssetRegistry.Providers
//{
//    public class AddressableAssetProvider<T> : AssetProvider<T> where T : Object
//    {
//        private readonly string key;

//        public string Key
//        {
//            get
//            {
//                return key;
//            }
//        }

//        public AddressableAssetProvider(string key)
//        {
//            this.key = key;
//        }

//        protected override async UniTask<object> LoadInnerAsync()
//        {
//            return await Addressables.LoadAssetAsync<T>(Key).Task;
//        }

//        public override void Unload()
//        {
//            Addressables.Release(TypedAsset);
//            Asset = null;
//        }
//    }
//}
