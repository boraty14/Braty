using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Braty.Core.Runtime.Scripts.GameTasks
{
    public class GameTaskRunner : IGameTaskRunner
    {
        private readonly List<GameTask> _gameTasks = new();
        private bool _isRunning;

        public GameTaskRunner()
        {
            RunGameTasks().Forget();
        }

        void IGameTaskRunner.Enqueue(GameTaskDelegate uniTask)
        {
            EnqueueTask(new GameTask(uniTask));
        }
        
        void IGameTaskRunner.Enqueue(GameTask gameTask)
        {
            EnqueueTask(gameTask);   
        }
        
        void IGameTaskRunner.Enqueue(Action action)
        {
            EnqueueTask(new GameTask(ExecuteAction));
            return;

            async UniTask ExecuteAction()
            {
                action?.Invoke();
                await UniTask.CompletedTask;
            }
        }
        
        void IGameTaskRunner.ClearQueue() => _gameTasks.Clear();

        private void EnqueueTask(GameTask gameTask)
        {
            int gameTaskCount = _gameTasks.Count;
            for (int i = 0; i < gameTaskCount; i++)
            {
                if (_gameTasks[i].Priority >= gameTask.Priority)
                {
                    continue;
                }
                _gameTasks.Insert(i,gameTask);
                return;
            }
            _gameTasks.Add(gameTask);
        }

        private async UniTaskVoid RunGameTasks()
        {
            if (_isRunning)
            {
                return;
            }
            
            _isRunning = true;
            
            while (Application.isPlaying)
            {
                if (_gameTasks.Count == 0)
                {
                    await UniTask.Yield();
                    continue;
                }

                var currentGameTask = _gameTasks[0];
                _gameTasks.RemoveAt(0);
                
                try
                {
                    if (currentGameTask.ExecuteAction != null)
                    {
                        await currentGameTask.ExecuteAction();
                    }
                }
                catch (Exception error)
                {
                    Debug.LogError($"Game task runner failed with error: {error}");
                    if (currentGameTask.HandleErrorAction != null)
                    {
                        await currentGameTask.HandleErrorAction();
                    }
                }
            }
            
            _isRunning = false;
        }
    }
}