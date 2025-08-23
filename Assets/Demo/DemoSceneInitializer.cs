using Braty.Core.Runtime.Scripts.BUI;
using Braty.Core.Runtime.Scripts.MonoEcs;
using Braty.Core.Runtime.Scripts.Scene;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Demo
{
    public class DemoSceneInitializer : MonoBehaviour
    {
        private void Start()
        {
            MonoManager.Init();
            SceneLoader.Init();
            BPanelManager.Init();

            LoadNextDemoScene().Forget();
        }

        private async UniTaskVoid LoadNextDemoScene()
        {
            await SceneLoader.LoadSceneAsync("DemoScene");
            Debug.LogError(2);
        }
    }
}