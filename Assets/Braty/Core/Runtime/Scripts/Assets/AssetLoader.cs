using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Braty.Core.Runtime.Scripts.Assets
{
    public static class AssetLoader
    {
        private static readonly Dictionary<string, AsyncOperationHandle> _assetHandles = new();
        private static readonly Dictionary<Type, AsyncOperationHandle> _monoHandles = new();

        public static void Init()
        {
            _assetHandles.Clear();
            _monoHandles.Clear();
        }

        public static IEnumerator LoadAsset<T>(string key)
        {
            if (_assetHandles.ContainsKey(key))
            {
                yield break;
            }
            
            var handle = Addressables.LoadAssetAsync<T>(key);
            yield return handle;
            _assetHandles.TryAdd(key,handle);
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
        
        public static IEnumerator LoadMono<T>()
        {
            var key = typeof(T);
            if (_monoHandles.ContainsKey(key))
            {
                yield break;
            }
            
            var handle = Addressables.LoadAssetAsync<T>(key);
            yield return handle;
            _monoHandles.TryAdd(key,handle);
        }

        public static void UnloadMono<T>()
        {
            var key = typeof(T);
            Addressables.Release(_monoHandles[key]);
            _monoHandles.Remove(key);
        }

        public static T GetMonoInstance<T>(Transform monoParent = null) where T : MonoBehaviour
        {
            return UnityEngine.Object.Instantiate((T)_monoHandles[typeof(T)].Result);
        }
    }
}