using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Braty.Core.Runtime.Scripts.Pool
{
    public static class MonoPool
    {
        private static readonly Dictionary<Type, object> _monoPoolObjects = new();
        public static void Init() => _monoPoolObjects.Clear();
        
        public static void LoadPool<T>(T prefab, Transform poolParent, int initial = 10, int max = 100, bool collectionChecks = false) where T : MonoBehaviour
        {
            var monoPoolObjectKey = typeof(T);

            if (_monoPoolObjects.ContainsKey(monoPoolObjectKey))
            {
                Debug.LogError($"Asset {monoPoolObjectKey} already loaded");
                return;
            }
            
            var monoPoolObject = new MonoPoolDefinition<T>(
                prefab,
                new ObjectPool<T>(
                    CreateSetup<T>,
                    GetSetup<T>,
                    ReleaseSetup<T>,
                    DestroySetup<T>,
                    collectionChecks,
                    initial,
                    max),
                poolParent
            );
            
            _monoPoolObjects.TryAdd(monoPoolObjectKey, monoPoolObject);
        }
        
        public static async UniTask LoadPool<T>(Transform poolParent, int initial = 10, int max = 100, bool collectionChecks = false) where T : MonoBehaviour
        {
            var monoPoolObjectKey = typeof(T);

            if (_monoPoolObjects.ContainsKey(monoPoolObjectKey))
            {
                Debug.LogError($"Asset {monoPoolObjectKey} already loaded");
                return;
            }

            var assetHandle = Addressables.LoadAssetAsync<GameObject>(monoPoolObjectKey.Name);
            await assetHandle;
            if (!assetHandle.Result.TryGetComponent(out T prefab))
            {
                Debug.LogError($"Asset {monoPoolObjectKey} does not contain component");
                return;
            }

            var monoPoolObject = new MonoPoolDefinition<T>(
                prefab,
                new ObjectPool<T>(
                    CreateSetup<T>,
                    GetSetup<T>,
                    ReleaseSetup<T>,
                    DestroySetup<T>,
                    collectionChecks,
                    initial,
                    max),
                poolParent
            );

            _monoPoolObjects.TryAdd(monoPoolObjectKey, monoPoolObject);
        }

        public static T Get<T>() where T : MonoBehaviour
        {
            var monoPoolObject = (MonoPoolDefinition<T>)_monoPoolObjects[typeof(T)];
            var mono = monoPoolObject.Pool.Get();
            return mono;
        }
        
        public static void Release<T>(T obj) where T : MonoBehaviour
        {
            var monoPoolObject = (MonoPoolDefinition<T>)_monoPoolObjects[typeof(T)];
            monoPoolObject.Pool.Release(obj);
        }
        
        private static T CreateSetup<T>() where T : MonoBehaviour
        {
            var monoPoolObject = (MonoPoolDefinition<T>)_monoPoolObjects[typeof(T)];
            return Object.Instantiate(monoPoolObject.Prefab, monoPoolObject.Parent);
        }

        private static void GetSetup<T>(T unit) where T : MonoBehaviour => unit.gameObject.SetActive(true);
        private static void ReleaseSetup<T>(T unit) where T : MonoBehaviour => unit.gameObject.SetActive(false);
        private static void DestroySetup<T>(T unit) where T : MonoBehaviour => Object.Destroy(unit.gameObject);
    }
}