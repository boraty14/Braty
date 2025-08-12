using Braty.Core.Runtime.Scripts.Scene;
using UnityEngine;

namespace Demo
{
    public class DemoNextSceneInitializer : SceneInitializer
    {
        protected override void OnAwake()
        {
            Debug.LogError(1);
        }
    }
}