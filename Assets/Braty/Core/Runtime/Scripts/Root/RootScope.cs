using Braty.Core.Runtime.Scripts.Assets;
using Braty.Core.Runtime.Scripts.Audio;
using Braty.Core.Runtime.Scripts.Cameras;
using Braty.Core.Runtime.Scripts.GameRoutines;
using Braty.Core.Runtime.Scripts.GameTasks;
using Braty.Core.Runtime.Scripts.MonoEvents;
using Braty.Core.Runtime.Scripts.Panels;
using Braty.Core.Runtime.Scripts.Pool;
using Braty.Core.Runtime.Scripts.Save;
using Braty.Core.Runtime.Scripts.Scene;
using Braty.Core.Runtime.Scripts.Signals;
using Braty.Core.Runtime.Scripts.State;
using Reflex.Core;
using UnityEngine;

namespace Braty.Core.Runtime.Scripts.Root
{
    public class RootScope : MonoBehaviour, IInstaller
    {
        [SerializeField] private PanelManager _panelManager;
        [SerializeField] private GameRoutineRunner _gameRoutineRunner;
        [SerializeField] private MonoEventDispatcher _monoEventDispatcher;
        [SerializeField] private MonoStateManager _monoStateManager;
        [SerializeField] private SoundManager _soundManager;
        [SerializeField] private MusicManager _musicManager;
        [SerializeField] private RootCamera _rootCamera;

        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            containerBuilder.AddSingleton(_panelManager,typeof(IPanelManager));
            containerBuilder.AddSingleton(_gameRoutineRunner,typeof(IGameRoutineRunner));
            containerBuilder.AddSingleton(_monoEventDispatcher,typeof(IMonoEventDispatcher));
            containerBuilder.AddSingleton(_monoStateManager,typeof(IMonoStateManager));
            containerBuilder.AddSingleton(_soundManager,typeof(ISoundManager));
            containerBuilder.AddSingleton(_musicManager,typeof(IMusicManager));
            containerBuilder.AddSingleton(_rootCamera,typeof(IRootCamera));
            containerBuilder.AddSingleton(typeof(GameTaskRunner),typeof(IGameTaskRunner));
            containerBuilder.AddSingleton(typeof(MonoPool),typeof(IMonoPool));
            containerBuilder.AddSingleton(typeof(SaveManager),typeof(ISaveManager));
            containerBuilder.AddSingleton(typeof(SceneLoader),typeof(ISceneLoader));
            containerBuilder.AddSingleton(typeof(SignalBus),typeof(ISignalBus));
            containerBuilder.AddSingleton(typeof(AssetLoader),typeof(IAssetLoader));
        }
    }
}