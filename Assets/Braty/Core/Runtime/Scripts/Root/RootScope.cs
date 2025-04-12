using Braty.Core.Runtime.Scripts.Assets;
using Braty.Core.Runtime.Scripts.Audio;
using Braty.Core.Runtime.Scripts.Cameras;
using Braty.Core.Runtime.Scripts.GameRoutines;
using Braty.Core.Runtime.Scripts.GameTasks;
using Braty.Core.Runtime.Scripts.MonoEvents;
using Braty.Core.Runtime.Scripts.Panels;
using Braty.Core.Runtime.Scripts.Pool;
using Braty.Core.Runtime.Scripts.Scene;
using Braty.Core.Runtime.Scripts.Signals;
using Braty.Core.Runtime.Scripts.State;
using Reflex.Core;
using UnityEngine;

namespace Braty.Core.Runtime.Scripts.Root
{
    public class RootScope : MonoBehaviour, IInstaller
    {
        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            containerBuilder.AddSingleton(typeof(GameTaskRunner), typeof(IGameTaskRunner));
            containerBuilder.AddSingleton(typeof(MonoStateManager), typeof(IMonoStateManager));
            containerBuilder.AddSingleton(typeof(GameRoutineRunner), typeof(IGameRoutineRunner));
            containerBuilder.AddSingleton(typeof(MonoEventDispatcher), typeof(IMonoEventDispatcher));
            containerBuilder.AddSingleton(typeof(SoundManager), typeof(ISoundManager));
            containerBuilder.AddSingleton(typeof(MusicManager), typeof(IMusicManager));
            containerBuilder.AddSingleton(typeof(PanelManager), typeof(IPanelManager));
            containerBuilder.AddSingleton(typeof(RootCamera), typeof(IRootCamera));
            containerBuilder.AddSingleton(typeof(MonoPool), typeof(IMonoPool));
            containerBuilder.AddSingleton(typeof(SceneLoader), typeof(ISceneLoader));
            containerBuilder.AddSingleton(typeof(SignalBus), typeof(ISignalBus));
            containerBuilder.AddSingleton(typeof(AssetLoader), typeof(IAssetLoader));
        }
    }
}