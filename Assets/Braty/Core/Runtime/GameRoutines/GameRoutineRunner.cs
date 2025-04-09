using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Braty.Core.Runtime.GameRoutines
{
    public class GameRoutineRunner : MonoBehaviour, IGameRoutineRunner
    {
        private readonly Dictionary<string, List<Coroutine>> _runningTagGameRoutines = new();
        
        void IGameRoutineRunner.RunIndependentRoutine(IEnumerator gameRoutine, string routineTag)
        {
            StartCoroutine(RunRoutineInternal(gameRoutine,routineTag));
        }

        void IGameRoutineRunner.CancelRunningTagRoutines(string routineTag)
        {
            if (!_runningTagGameRoutines.ContainsKey(routineTag))
            {
                return;
            }

            foreach (var tagRoutine in _runningTagGameRoutines[routineTag])
            {
                StopCoroutine(tagRoutine);
            }
            _runningTagGameRoutines[routineTag].Clear();
        }

        void IGameRoutineRunner.CancelRunningAllRoutines()
        {
            foreach (var tagRoutines in _runningTagGameRoutines)
            {
                foreach (var routine in tagRoutines.Value)
                {
                    StopCoroutine(routine);
                }
                tagRoutines.Value.Clear();
            }
            _runningTagGameRoutines.Clear();
        }
        
        private IEnumerator RunRoutineInternal(IEnumerator gameRoutine,string routineTag)
        {
            if (!_runningTagGameRoutines.ContainsKey(routineTag))
            {
                _runningTagGameRoutines[routineTag] = new List<Coroutine>();
            }

            var coroutine = StartCoroutine(gameRoutine);
            _runningTagGameRoutines[routineTag].Add(coroutine);
            yield return coroutine;
            _runningTagGameRoutines[routineTag].Remove(coroutine);
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}