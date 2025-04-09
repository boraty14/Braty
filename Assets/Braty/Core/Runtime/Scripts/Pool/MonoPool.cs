using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Braty.Core.Runtime.Scripts.Pool
{
    public class MonoPool : IMonoPool
    {
        private readonly Dictionary<Type, MonoPoolObject> _monoPoolObjects = new();

        async UniTask IMonoPool.LoadPool<T>(Transform poolParent, int initial, int max, bool collectionChecks)
        {
            var monoPoolObjectKey = typeof(T);

            if (_monoPoolObjects.ContainsKey(monoPoolObjectKey))
            {
                Debug.LogError($"Asset {monoPoolObjectKey} already loaded");
                return;
            }

            var asset = await Addressables.LoadAssetAsync<GameObject>(monoPoolObjectKey.Name).ToUniTask();
            if (!asset.TryGetComponent(out T prefab))
            {
                Debug.LogError($"Asset {monoPoolObjectKey} does not contain component");
                return;
            }

            var monoPoolObject = new MonoPoolObject<T>(
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

        T IMonoPool.Get<T>()
        {
            var monoPoolObject = (MonoPoolObject<T>)_monoPoolObjects[typeof(T)];
            return monoPoolObject.Pool.Get();
        }

        void IMonoPool.Release<T>(T obj)
        {
            var monoPoolObject = (MonoPoolObject<T>)_monoPoolObjects[typeof(T)];
            monoPoolObject.Pool.Release(obj);
        }


        private T CreateSetup<T>() where T : MonoBehaviour
        {
            var monoPoolObject = (MonoPoolObject<T>)_monoPoolObjects[typeof(T)];
            return Object.Instantiate(monoPoolObject.Prefab, monoPoolObject.Parent);
        }

        private void GetSetup<T>(T unit) where T : MonoBehaviour => unit.gameObject.SetActive(true);
        private void ReleaseSetup<T>(T unit) where T : MonoBehaviour => unit.gameObject.SetActive(false);
        private void DestroySetup<T>(T unit) where T : MonoBehaviour => Object.Destroy(unit.gameObject);
    }
}