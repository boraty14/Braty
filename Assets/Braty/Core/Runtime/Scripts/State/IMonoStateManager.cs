namespace Braty.Core.Runtime.Scripts.State
{
    public interface IMonoStateManager
    { 
        T Get<T>() where T : MonoState;
    }
}