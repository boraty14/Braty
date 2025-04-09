using System.Collections;

namespace Braty.Core.Runtime.GameRoutines
{
    public interface IGameRoutineRunner
    {
        void RunIndependentRoutine(IEnumerator gameRoutine, string routineTag = "");
        void CancelRunningTagRoutines(string routineTag);
        void CancelRunningAllRoutines();
    }
}