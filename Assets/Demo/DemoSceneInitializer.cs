using Braty.Core.Runtime.Scripts.MonoEcs;
using Braty.Core.Runtime.Scripts.Panels;
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
            PanelManager.Init();

            LoadNextDemoScene().Forget();
        }

        private async UniTaskVoid LoadNextDemoScene()
        {
            await SceneLoader.LoadSceneAsync("DemoScene");
            Debug.LogError(2);
        }
    }
}