using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Braty.Core.Runtime.Scripts.Assets
{
    public interface IAssetLoader
    {
        UniTask<T> LoadAsset<T>(string key);
        void UnloadAsset<T>(string key);

        UniTask<T> InstantiateMonoBehaviour<T>(Transform parent) where T : AssetBase;
        void ReleaseMonoBehaviour<T>(T monoBehaviour) where T : AssetBase;
    }
}