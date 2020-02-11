using System;
using Exanite.Core.Extensions;
using UniRx.Async;

namespace Exanite.Arpg.AssetRegistry.Providers
{
    public abstract class AssetProvider
    {
        private readonly Type assetType;
        private object asset;
        private int referenceCount;

        private UniTask loadOperation;

        public Type AssetType
        {
            get
            {
                return assetType;
            }
        }

        public object Asset
        {
            get
            {
                return asset;
            }

            protected set
            {
                asset = value;
            }
        }

        public bool IsLoaded
        {
            get
            {
                return Asset == null;
            }
        }

        public bool IsLoading
        {
            get
            {
                return loadOperation.Status == AwaiterStatus.Pending;
            }
        }

        public int ReferenceCount
        {
            get
            {
                return referenceCount;
            }

            protected set
            {
                referenceCount = value;
            }
        }

        public AssetProvider(Type assetType)
        {
            this.assetType = assetType ?? throw new ArgumentNullException(nameof(assetType));
        }

        public T Get<T>()
        {
            if (IsLoaded)
            {
                referenceCount++;

                return (T)asset;
            }
            else
            {
                throw new InvalidOperationException("Asset has not yet been loaded");
            }
        }

        public async UniTask<T> GetAsync<T>()
        {
            if (!IsLoaded)
            {
                if (!IsLoading)
                {
                    loadOperation = LoadAsync();
                }

                await loadOperation;
            }

            referenceCount++;

            return (T)Asset;
        }

        public void Release()
        {
            referenceCount--;

            if (referenceCount == 0)
            {
                Unload();
            }
            else if (referenceCount < 0)
            {
                throw new InvalidOperationException("Get and Release calls must be balanced");
            }
        }

        public virtual void Unload()
        {
            Asset = null;
        }

        protected async UniTask LoadAsync()
        {
            if (IsLoaded)
            {
                return;
            }

            object loadedObject = await LoadInnerAsync();

            if (ValidateAssetType(loadedObject))
            {
                Asset = loadedObject;
            }
            else
            {
                Asset = AssetType.GetDefault();
                throw new InvalidAssetTypeException(
                    $"Expected to load an asset of type '{AssetType.Name}', but loaded asset type of type '{loadedObject.GetType().Name}'",
                    AssetType, loadedObject.GetType());
            }
        }

        protected abstract UniTask<object> LoadInnerAsync();

        protected bool ValidateAssetType(object asset)
        {
            Type type = asset?.GetType();
            return AssetType.IsAssignableFrom(type);
        }
    }
}
