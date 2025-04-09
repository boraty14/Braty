using System;

namespace Braty.Core.Runtime.Scripts.GameTasks
{
    public interface IGameTaskRunner
    {
        void Enqueue(GameTaskDelegate uniTask);
        void Enqueue(GameTask gameTask);
        void Enqueue(Action action);
        void ClearQueue();
    }
}