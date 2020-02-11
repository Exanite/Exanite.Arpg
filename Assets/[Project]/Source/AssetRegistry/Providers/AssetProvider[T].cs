using UniRx.Async;

namespace Exanite.Arpg.AssetRegistry.Providers
{
    public abstract class AssetProvider<T> : AssetProvider
    {
        public T TypedAsset
        {
            get
            {
                return (T)Asset;
            }
        }

        public AssetProvider() : base(typeof(T)) { }

        public T Get()
        {
            return Get<T>();
        }

        public UniTask<T> GetAsync()
        {
            return GetAsync<T>();
        }
    }
}
