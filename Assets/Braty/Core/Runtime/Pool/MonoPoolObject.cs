using UnityEngine;
using UnityEngine.Pool;

namespace Braty.Core.Runtime.Pool
{
    public abstract class MonoPoolObject
    {
        
    }
    
    public class MonoPoolObject<T> : MonoPoolObject where T : MonoBehaviour
    {
        public readonly T Prefab;
        public readonly ObjectPool<T> Pool;
        public readonly Transform Parent;

        public MonoPoolObject(T prefab, ObjectPool<T> pool, Transform parent)
        {
            Prefab = prefab;
            Pool = pool;
            Parent = parent;
        }
    }
}