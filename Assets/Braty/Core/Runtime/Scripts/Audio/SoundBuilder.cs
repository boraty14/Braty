using UnityEngine;

namespace Braty.Core.Runtime.Scripts.Audio
{
    public class SoundBuilder
    {
        private readonly SoundSystem _soundSystem;
        private Vector3 _position = Vector3.zero;
        private bool _randomPitch;

        public SoundBuilder(SoundSystem soundSystem)
        {
            _soundSystem = soundSystem;
        }

        public SoundBuilder WithPosition(Vector3 position)
        {
            _position = position;
            return this;
        }

        public SoundBuilder WithRandomPitch()
        {
            _randomPitch = true;
            return this;
        }

        public void Play(SoundData soundData)
        {
            if (soundData == null)
            {
                Debug.LogError("SoundData is null");
                return;
            }

            if (!_soundSystem.CanPlaySound(soundData)) return;

            SoundEmitter soundEmitter = _soundSystem.Get();
            soundEmitter.Initialize(soundData);
            soundEmitter.transform.position = _position;
            soundEmitter.transform.parent = _soundSystem.transform;

            if (_randomPitch)
            {
                soundEmitter.WithRandomPitch();
            }

            if (soundData.frequentSound)
            {
                soundEmitter.Node = _soundSystem.FrequentSoundEmitters.AddLast(soundEmitter);
            }

            soundEmitter.Play();
        }
    }
}