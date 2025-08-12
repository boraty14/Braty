using Braty.Core.Runtime.Scripts.MonoEcs;

namespace Braty.Core.Runtime.Scripts.Utils
{
    using UnityEngine;
    using UnityEngine.Pool;

    /// <summary>
    /// A simple base class to simplify object pooling in Unity 2021.
    /// Derive from this class, call InitPool and you can Get and Release to your hearts content.
    /// If you enjoyed the video or this script, make sure you give me a like on YT and let me know what you thought :)
    /// </summary>
    /// <typeparam name="T">A MonoBehaviour object you'd like to perform pooling on.</typeparam>
    public abstract class PoolerBase<T> : MonoSystem where T : MonoBehaviour 
    {
        [SerializeField] private T _prefab;
        protected ObjectPool<T> Pool { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            InitPool();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Pool?.Dispose();
        }

        protected void InitPool(int initial = 10, int max = 20, bool collectionChecks = false) {
            Pool = new ObjectPool<T>(
                CreateSetup,
                GetSetup,
                ReleaseSetup,
                DestroySetup,
                collectionChecks,
                initial,
                max);
        }

        #region Overrides
        protected virtual T CreateSetup() => Instantiate(_prefab);
        protected virtual void GetSetup(T obj) => obj.gameObject.SetActive(true);
        protected virtual void ReleaseSetup(T obj) => obj.gameObject.SetActive(false);
        protected virtual void DestroySetup(T obj) => Destroy(obj);
        #endregion

        #region Getters
        public T Get() => Pool.Get();
        public void Release(T obj) => Pool.Release(obj);
        #endregion
    }
}