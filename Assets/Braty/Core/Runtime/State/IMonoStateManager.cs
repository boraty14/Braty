namespace Braty.Core.Runtime.State
{
    public interface IMonoStateManager
    { 
        T Get<T>() where T : MonoState;
    }
}