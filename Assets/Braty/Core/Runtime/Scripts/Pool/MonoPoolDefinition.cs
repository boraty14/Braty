using UnityEngine;
using UnityEngine.Pool;

namespace Braty.Core.Runtime.Scripts.Pool
{
    public struct MonoPoolDefinition<T> where T : MonoBehaviour
    {
        public readonly T Prefab;
        public readonly ObjectPool<T> Pool;
        public readonly Transform Parent;

        public MonoPoolDefinition(T prefab, ObjectPool<T> pool, Transform parent)
        {
            Prefab = prefab;
            Pool = pool;
            Parent = parent;
        }
    }
}