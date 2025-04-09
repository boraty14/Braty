using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Reflex.Core;
using Reflex.Extensions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Braty.Core.Runtime.Scene
{
    public class SceneLoader : ISceneLoader
    {
        private readonly Dictionary<string, SceneInstance> _loadedScenes = new Dictionary<string, SceneInstance>();
        private readonly Dictionary<string, AsyncOperationHandle<SceneInstance>> _loadingOperations = new Dictionary<string, AsyncOperationHandle<SceneInstance>>();

        private const string RootScene = "RootScene";
        
        /// <summary>
        /// Loads a scene additively using its addressable key
        /// </summary>
        /// <param name="sceneKey">The addressable key of the scene</param>
        /// <param name="activateOnLoad">Whether to activate the scene immediately after loading</param>
        /// <returns>True if the scene was loaded successfully</returns>
        async UniTask<bool> ISceneLoader.LoadSceneAsync(string sceneKey, bool activateOnLoad)
        {
            try
            {
                // Check if scene is already loaded
                if (_loadedScenes.ContainsKey(sceneKey))
                {
                    Debug.LogWarning($"Scene {sceneKey} is already loaded!");
                    return true;
                }

                // Check if scene is currently loading
                if (_loadingOperations.ContainsKey(sceneKey))
                {
                    Debug.LogWarning($"Scene {sceneKey} is already being loaded!");
                    await WaitForSceneLoad(sceneKey);
                    return true;
                }

                // Start loading the scene
                var loadOperation = Addressables.LoadSceneAsync(sceneKey, LoadSceneMode.Additive, false);
                _loadingOperations[sceneKey] = loadOperation;

                // Wait for the scene to load
                var sceneInstance = await loadOperation.Task;
                
                // Override new scene parent scope
                var bootScene = SceneManager.GetSceneByName(RootScene);
                ReflexSceneManager.OverrideSceneParentContainer(scene: loadOperation.Result.Scene, parent: bootScene.GetSceneContainer());
                if (activateOnLoad)
                {
                    await loadOperation.Result.ActivateAsync();
                }
            
                // Store the loaded scene
                _loadedScenes[sceneKey] = sceneInstance;
                _loadingOperations.Remove(sceneKey);

                Debug.Log($"Successfully loaded scene: {sceneKey}");
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load scene {sceneKey}: {e.Message}");
                _loadingOperations.Remove(sceneKey);
                return false;
            }
        }

        /// <summary>
        /// Unloads a previously loaded addressable scene
        /// </summary>
        /// <param name="sceneKey">The addressable key of the scene to unload</param>
        /// <returns>True if the scene was unloaded successfully</returns>
        async UniTask<bool> ISceneLoader.UnloadSceneAsync(string sceneKey)
        {
            try
            {
                if (!_loadedScenes.ContainsKey(sceneKey))
                {
                    Debug.LogWarning($"Scene {sceneKey} is not loaded!");
                    return false;
                }

                var sceneInstance = _loadedScenes[sceneKey];
                var unloadOperation = Addressables.UnloadSceneAsync(sceneInstance);
                await unloadOperation.Task;

                _loadedScenes.Remove(sceneKey);
                Debug.Log($"Successfully unloaded scene: {sceneKey}");
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to unload scene {sceneKey}: {e.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Checks if a scene is currently loaded
        /// </summary>
        /// <param name="sceneKey">The addressable key of the scene</param>
        /// <returns>True if the scene is loaded</returns>
        bool ISceneLoader.IsSceneLoaded(string sceneKey)
        {
            return _loadedScenes.ContainsKey(sceneKey);
        }

        /// <summary>
        /// Gets all currently loaded scene keys
        /// </summary>
        /// <returns>Array of loaded scene keys</returns>
        string[] ISceneLoader.GetLoadedScenes()
        {
            return new List<string>(_loadedScenes.Keys).ToArray();
        }

        /// <summary>
        /// Waits for a scene that is currently being loaded to complete
        /// </summary>
        /// <param name="sceneKey">The addressable key of the scene</param>
        private async UniTask WaitForSceneLoad(string sceneKey)
        {
            if (_loadingOperations.TryGetValue(sceneKey, out var operation))
            {
                await operation.Task;
            }
        }
    }
}