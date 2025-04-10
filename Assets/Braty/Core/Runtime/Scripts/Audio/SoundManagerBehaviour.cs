using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Braty.Core.Runtime.Scripts.Audio
{
    public class SoundManagerBehaviour : MonoBehaviour
    {
        [SerializeField] private SoundEmitter _soundEmitterPrefab;
        [SerializeField] private bool _collectionCheck = true;
        [SerializeField] private int _defaultCapacity = 10;
        [SerializeField] private int _maxPoolSize = 100;
        [SerializeField] private int _maxSoundInstances = 30;
        
        private readonly List<SoundEmitter> _activeSoundEmitters = new();
        private readonly LinkedList<SoundEmitter> _frequentSoundEmitters = new();
        private IObjectPool<SoundEmitter> _soundEmitterPool;

        public SoundEmitter Get() => _soundEmitterPool.Get();
        public void Release(SoundEmitter soundEmitter) => _soundEmitterPool.Release(soundEmitter);

        public bool CanPlaySound(SoundData data)
        {
            if (!data.frequentSound) return true;

            if (_frequentSoundEmitters.Count >= _maxSoundInstances)
            {
                try
                {
                    _frequentSoundEmitters.First.Value.Stop();
                    return true;
                }
                catch
                {
                    Debug.Log("SoundEmitter is already released");
                }

                return false;
            }

            return true;
        }

        public void StopAll()
        {
            foreach (var soundEmitter in _activeSoundEmitters)
            {
                soundEmitter.Stop();
            }

            _frequentSoundEmitters.Clear();
        }

        public LinkedListNode<SoundEmitter> AddFrequentSoundEmitter(SoundEmitter soundEmitter)
        {
            return _frequentSoundEmitters.AddLast(soundEmitter);
        }

        private void Awake()
        {
            InitializePool();
        }

        private void InitializePool()
        {
            _soundEmitterPool = new ObjectPool<SoundEmitter>(
                CreateSoundEmitter,
                OnTakeFromPool,
                OnReturnedToPool,
                OnDestroyPoolObject,
                _collectionCheck,
                _defaultCapacity,
                _maxPoolSize);
        }

        private SoundEmitter CreateSoundEmitter()
        {
            var soundEmitter = Instantiate(_soundEmitterPrefab);
            soundEmitter.gameObject.SetActive(false);
            return soundEmitter;
        }

        private void OnTakeFromPool(SoundEmitter soundEmitter)
        {
            soundEmitter.gameObject.SetActive(true);
            _activeSoundEmitters.Add(soundEmitter);
        }

        private void OnReturnedToPool(SoundEmitter soundEmitter)
        {
            if (soundEmitter.Node != null)
            {
                _frequentSoundEmitters.Remove(soundEmitter.Node);
                soundEmitter.Node = null;
            }

            soundEmitter.gameObject.SetActive(false);
            _activeSoundEmitters.Remove(soundEmitter);
        }

        private void OnDestroyPoolObject(SoundEmitter soundEmitter)
        {
            Destroy(soundEmitter.gameObject);
        }
    }
}