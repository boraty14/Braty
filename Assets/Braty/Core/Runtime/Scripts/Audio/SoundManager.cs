using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Braty.Core.Runtime.Scripts.Audio
{
    public class SoundManager : MonoBehaviour, ISoundManager
    {
        [SerializeField] private SoundEmitter _soundEmitterPrefab;
        [SerializeField] private bool _collectionCheck = true;
        [SerializeField] private int _defaultCapacity = 10;
        [SerializeField] private int _maxPoolSize = 100;
        [SerializeField] private int _maxSoundInstances = 30;

        private IObjectPool<SoundEmitter> _soundEmitterPool;
        private readonly List<SoundEmitter> _activeSoundEmitters = new();
        private readonly LinkedList<SoundEmitter> _frequentSoundEmitters = new();

        private void Awake()
        {
            InitializePool();
        }

        SoundBuilder ISoundManager.CreateSoundBuilder() => new SoundBuilder(this);

        bool ISoundManager.CanPlaySound(SoundData data)
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

        SoundEmitter ISoundManager.Get()
        {
            return _soundEmitterPool.Get();
        }

        void ISoundManager.ReturnToPool(SoundEmitter soundEmitter)
        {
            _soundEmitterPool.Release(soundEmitter);
        }

        void ISoundManager.StopAll()
        {
            foreach (var soundEmitter in _activeSoundEmitters)
            {
                soundEmitter.Stop();
            }

            _frequentSoundEmitters.Clear();
        }

        LinkedList<SoundEmitter> ISoundManager.GetFrequentSoundEmitters() => _frequentSoundEmitters;
        Transform ISoundManager.GetSoundTransform() => transform;

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