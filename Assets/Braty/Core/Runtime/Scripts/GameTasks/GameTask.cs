using Cysharp.Threading.Tasks;

namespace Braty.Core.Runtime.Scripts.GameTasks
{
    public delegate UniTask GameTaskDelegate();
    
    public class GameTask
    {
        public readonly GameTaskDelegate ExecuteAction;
        public readonly GameTaskDelegate HandleErrorAction;
        public readonly int Priority;

        public GameTask(GameTaskDelegate executeAction, GameTaskDelegate handleErrorAction = null, int priority = 0)
        {
            ExecuteAction = executeAction;
            HandleErrorAction = handleErrorAction;
            Priority = priority;
        }
    }
}