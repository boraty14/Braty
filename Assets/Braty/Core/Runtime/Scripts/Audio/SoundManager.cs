using System.Collections.Generic;
using UnityEngine;

namespace Braty.Core.Runtime.Scripts.Audio
{
    public class SoundManager : ISoundManager
    {
        private readonly SoundManagerBehaviour _soundManagerBehaviour;

        public SoundManager()
        {
            _soundManagerBehaviour = Resources.Load<SoundManagerBehaviour>("SoundManagerBehaviour");
        }

        SoundBuilder ISoundManager.CreateSoundBuilder() => new SoundBuilder(this);

        bool ISoundManager.CanPlaySound(SoundData data)
        {
            return _soundManagerBehaviour.CanPlaySound(data);
        }

        SoundEmitter ISoundManager.Get()
        {
            return _soundManagerBehaviour.Get();
        }

        void ISoundManager.ReturnToPool(SoundEmitter soundEmitter)
        {
            _soundManagerBehaviour.Release(soundEmitter);
        }

        void ISoundManager.StopAll()
        {
            _soundManagerBehaviour.StopAll();
        }

        LinkedListNode<SoundEmitter> ISoundManager.AddFrequentSoundEmitter(SoundEmitter soundEmitter) => _soundManagerBehaviour.AddFrequentSoundEmitter(soundEmitter);
        Transform ISoundManager.GetSoundTransform() => _soundManagerBehaviour.transform;
    }
}