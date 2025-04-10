namespace Braty.Core.Runtime.Scripts.State
{
    public interface IMonoStateManager
    {
        void Add<T>() where T : MonoState;
        T Get<T>() where T : MonoState;
    }
}