using Cysharp.Threading.Tasks;

namespace Braty.Core.Runtime.Scene
{
    public interface ISceneLoader
    {
        UniTask<bool> LoadSceneAsync(string sceneKey, bool activateOnLoad = true);
        UniTask<bool> UnloadSceneAsync(string sceneKey);
        bool IsSceneLoaded(string sceneKey);
        string[] GetLoadedScenes();
    }
}