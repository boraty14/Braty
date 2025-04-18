using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace Braty.Core.Runtime.Scripts.Pool
{
    public static class MonoPool
    {
        private static readonly Dictionary<Type, object> _monoPoolObjects = new();

        public static void Init() => _monoPoolObjects.Clear();

        public static void LoadPool<T>(Transform poolParent, int initial, int max, bool collectionChecks) where T : MonoBehaviour
        {
            var monoPoolObjectKey = typeof(T);

            if (_monoPoolObjects.ContainsKey(monoPoolObjectKey))
            {
                Debug.LogError($"Asset {monoPoolObjectKey} already loaded");
                return;
            }

            var asset = Addressables.LoadAssetAsync<GameObject>(monoPoolObjectKey.Name);
            asset.Completed += OnPoolPrefabLoaded;
            return;

            void OnPoolPrefabLoaded(AsyncOperationHandle<GameObject> handle)
            {
                asset.Completed -= OnPoolPrefabLoaded;
                if (!handle.Result.TryGetComponent(out T prefab))
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
        }



        public static T Get<T>() where T : MonoBehaviour
        {
            var monoPoolObject = (MonoPoolDefinition<T>)_monoPoolObjects[typeof(T)];
            return monoPoolObject.Pool.Get();
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