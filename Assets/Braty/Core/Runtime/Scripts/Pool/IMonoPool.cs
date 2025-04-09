using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Braty.Core.Runtime.Scripts.Pool
{
    public interface IMonoPool
    {
        UniTask LoadPool<T>(Transform poolParent, int initial = 1, int max = 100,
            bool collectionChecks = false)
            where T : MonoBehaviour;
        T Get<T>() where T : MonoBehaviour;
        void Release<T>(T obj) where T : MonoBehaviour;
    }
}