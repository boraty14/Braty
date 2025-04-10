using UnityEngine;

namespace Braty.Core.Runtime.Scripts.GameRoutines
{
    public class GameRoutineRunnerBehaviour : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
        
        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}