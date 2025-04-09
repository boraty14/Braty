using System;

namespace Braty.Core.Runtime.MonoEvents
{
    public interface IMonoEventDispatcher
    {
        event Action OnAppPause;
        event Action OnAppResume;
        event Action OnStart;
        event Action OnUpdate;
        event Action OnFixedUpdate; 
        event Action OnLateUpdate; 
    }
}