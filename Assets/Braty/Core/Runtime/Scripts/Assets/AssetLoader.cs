using System;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Braty.Core.Runtime.Scripts.Assets
{
    public static class AssetLoader
    {
        private static readonly Dictionary<string, AsyncOperationHandle> _assetHandles = new();

        public static void Init() => _assetHandles.Clear();

        public static void LoadAsset<T>(string key, Action onAssetLoaded)
        {
            if (_assetHandles.ContainsKey(key))
            {
                onAssetLoaded?.Invoke();
                return;
            }

            var handle = Addressables.LoadAssetAsync<T>(key);
            handle.Completed += OnAssetLoaded;

            
            void OnAssetLoaded(AsyncOperationHandle<T> asyncHandle)
            {
                handle.Completed -= OnAssetLoaded;
                _assetHandles.TryAdd(key, handle);
                onAssetLoaded?.Invoke();
            }
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