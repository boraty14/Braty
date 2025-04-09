using Cysharp.Threading.Tasks;

namespace Braty.Core.Runtime.Scripts.Scene
{
    public interface ISceneLoader
    {
        UniTask<bool> LoadSceneAsync(string sceneKey);
        UniTask<bool> UnloadSceneAsync(string sceneKey);
        bool IsSceneLoaded(string sceneKey);
        string[] GetLoadedScenes();
    }
}