using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Braty.Core.Runtime.Scripts.Scene
{
    public static class SceneLoader
    {
        private static readonly Dictionary<string, AsyncOperationHandle<SceneInstance>> _handles = new();

        public static void Init() => _handles.Clear();
        
        public static void LoadSceneAsync(string sceneKey, Action<AsyncOperationHandle<SceneInstance>> onSceneLoaded = null)
        {
            if (_handles.ContainsKey(sceneKey))
            {
                Debug.LogError($"{sceneKey} is already loaded");
                return;
            }
            
            var sceneLoadOperation = Addressables.LoadSceneAsync(sceneKey, LoadSceneMode.Additive);
            sceneLoadOperation.Completed += handle => _handles.TryAdd(sceneKey, handle);
            if (onSceneLoaded != null)
            {
                sceneLoadOperation.Completed += onSceneLoaded;
            }
        }

        public static void UnloadSceneAsync(string sceneKey)
        {
            if (!_handles.TryGetValue(sceneKey, out var handle))
            {
                Debug.LogError($"{sceneKey} is not loaded");
                return;
            }
            Addressables.UnloadSceneAsync(handle);
            _handles.Remove(sceneKey);
        }
        
        public static bool IsSceneLoaded(string sceneKey)
        {
            return _handles.ContainsKey(sceneKey);
        }
    }
}