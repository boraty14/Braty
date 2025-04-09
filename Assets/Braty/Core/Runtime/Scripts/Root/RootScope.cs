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
using UnityEngine.EventSystems;

namespace Braty.Core.Runtime.Scripts.Root
{
    public class RootScope : MonoBehaviour, IInstaller
    {
        [SerializeField] private EventSystem _eventSystem;
        [SerializeField] private PanelManager _panelManager;
        [SerializeField] private GameRoutineRunner _gameRoutineRunner;
        [SerializeField] private MonoEventDispatcher _monoEventDispatcher;
        [SerializeField] private MonoStateManager _monoStateManager;
        [SerializeField] private SoundManager _soundManager;
        [SerializeField] private MusicManager _musicManager;
        [SerializeField] private RootCamera _rootCamera;

        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            var rootScopeParent = new GameObject("Root Scope Parent").transform;
            Instantiate(_eventSystem, rootScopeParent);
            containerBuilder.AddSingleton(Instantiate(_panelManager,rootScopeParent),typeof(IPanelManager));
            containerBuilder.AddSingleton(Instantiate(_gameRoutineRunner,rootScopeParent),typeof(IGameRoutineRunner));
            containerBuilder.AddSingleton(Instantiate(_monoEventDispatcher,rootScopeParent),typeof(IMonoEventDispatcher));
            containerBuilder.AddSingleton(Instantiate(_monoStateManager,rootScopeParent),typeof(IMonoStateManager));
            containerBuilder.AddSingleton(Instantiate(_soundManager,rootScopeParent),typeof(ISoundManager));
            containerBuilder.AddSingleton(Instantiate(_musicManager,rootScopeParent),typeof(IMusicManager));
            containerBuilder.AddSingleton(Instantiate(_rootCamera,rootScopeParent),typeof(IRootCamera));
            
            containerBuilder.AddSingleton(typeof(GameTaskRunner),typeof(IGameTaskRunner));
            containerBuilder.AddSingleton(typeof(MonoPool),typeof(IMonoPool));
            containerBuilder.AddSingleton(typeof(SaveManager),typeof(ISaveManager));
            containerBuilder.AddSingleton(typeof(SceneLoader),typeof(ISceneLoader));
            containerBuilder.AddSingleton(typeof(SignalBus),typeof(ISignalBus));
            containerBuilder.AddSingleton(typeof(AssetLoader),typeof(IAssetLoader));
        }
    }
}