using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Braty.Core.Runtime.Scripts.Assets
{
    public static class AssetLoader
    {
        private static readonly Dictionary<string, AsyncOperationHandle> _assetHandles = new();

        public static void Init() => _assetHandles.Clear();

        public static async UniTask LoadAsset<T>(string key)
        {
            if (_assetHandles.ContainsKey(key))
            {
                return;
            }

            var handle = Addressables.LoadAssetAsync<T>(key);
            await handle;
            _assetHandles.TryAdd(key, handle);
        }

        public static void UnloadAsset<T>(string key)
        {
            Addressables.Release(_assetHandles[key]);
            _assetHandles.Remove(key);
        }

        public static T GetAsset<T>(string key)
        {
            return (T)_assetHandles[key].Result;
        }
    }
}