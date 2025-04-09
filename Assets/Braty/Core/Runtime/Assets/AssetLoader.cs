using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Braty.Core.Runtime.Assets
{
    public class AssetLoader : IAssetLoader
    {
        private readonly Dictionary<string, AssetLoadDefinition> _assetLoadDefinitions = new();
        
        async UniTask<T> IAssetLoader.LoadAsset<T>(string key)
        {
            if (_assetLoadDefinitions.TryGetValue(key, out var definition))
            {
                return (T)definition.Result;
            }
            
            var handle = Addressables.LoadAssetAsync<T>(key);
            var result = await handle.ToUniTask();
            _assetLoadDefinitions.TryAdd(key, new AssetLoadDefinition
            {
                Handle = handle,
                Result = result
            });
            return result;
        }

        void IAssetLoader.UnloadAsset<T>(string key)
        {
            Addressables.Release(_assetLoadDefinitions[key]);
            _assetLoadDefinitions.Remove(key);
        }

        async UniTask<T> IAssetLoader.InstantiateMonoBehaviour<T>(Transform parent)
        {
            var gameObject = await Addressables.InstantiateAsync(typeof(T),parent).ToUniTask();
            return gameObject.GetComponent<T>();
        }

        void IAssetLoader.ReleaseMonoBehaviour<T>(T monoBehaviour)
        {
            Addressables.ReleaseInstance(monoBehaviour.gameObject);
        }
    }
}