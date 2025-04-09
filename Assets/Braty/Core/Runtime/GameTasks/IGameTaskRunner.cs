using System;

namespace Braty.Core.Runtime.GameTasks
{
    public interface IGameTaskRunner
    {
        void Enqueue(GameTaskDelegate uniTask);
        void Enqueue(GameTask gameTask);
        void Enqueue(Action action);
        void ClearQueue();
    }
}