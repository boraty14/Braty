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
        private static readonly Dictionary<Type, object> _activeMonos = new();

        public static void Init()
        {
            _monoPoolObjects.Clear();
            _activeMonos.Clear();
        }

        public static IReadOnlyList<T> GetActiveMonos<T>() where T : MonoBehaviour
        {
            var monoType = typeof(T);
            if (!_activeMonos.ContainsKey(monoType))
            {
                _activeMonos.TryAdd(monoType, new List<T>());
            }

            return (IReadOnlyList<T>)_activeMonos[monoType];
        }
        
        public static void LoadPool<T>(Transform poolParent, Action onLoaded = null, int initial = 10, int max = 100, bool collectionChecks = false) where T : MonoBehaviour
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
                if (onLoaded != null)
                {
                    onLoaded?.Invoke();
                }
            }
        }

        public static T Get<T>() where T : MonoBehaviour
        {
            var monoPoolObject = (MonoPoolDefinition<T>)_monoPoolObjects[typeof(T)];
            var mono = monoPoolObject.Pool.Get();
            AddActiveMono(mono);
            return mono;
        }
        
        private static void AddActiveMono<T>(T monoInstance) where T : MonoBehaviour
        {
            var monoType = typeof(T);
            if (!_activeMonos.ContainsKey(monoType))
            {
                _activeMonos.TryAdd(monoType, new List<T>());
            }
            
            ((List<T>)_activeMonos[monoType]).Add(monoInstance);
        }

        public static void Release<T>(T obj) where T : MonoBehaviour
        {
            var monoPoolObject = (MonoPoolDefinition<T>)_monoPoolObjects[typeof(T)];
            monoPoolObject.Pool.Release(obj);
            RemoveActiveMono(obj);
        }
        
        private static void RemoveActiveMono<T>(T monoInstance) where T : MonoBehaviour
        {
            var monoType = typeof(T);
            if (!_activeMonos.ContainsKey(monoType))
            {
                Debug.LogError($"Mono list {monoType} is empty");
                return;
            }

            var monoList = (List<T>)_activeMonos[monoType];
            int monoIndex = monoList.IndexOf(monoInstance);
            if (monoIndex < 0)
            {
                Debug.LogError($"Mono is not in list {monoType}");
                return;
            }

            monoList[monoIndex] = monoList[^1];
            monoList.RemoveAt(monoList.Count - 1);
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