using UnityEngine;

namespace Braty.Core.Runtime.Audio
{
    public class SoundBuilder
    {
        private readonly ISoundManager _soundManager;
        private Vector3 _position = Vector3.zero;
        private bool _randomPitch;

        public SoundBuilder(ISoundManager soundManager)
        {
            _soundManager = soundManager;
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

            if (!_soundManager.CanPlaySound(soundData)) return;

            SoundEmitter soundEmitter = _soundManager.Get();
            soundEmitter.Initialize(soundData,_soundManager);
            soundEmitter.transform.position = _position;
            soundEmitter.transform.parent = _soundManager.GetSoundTransform();

            if (_randomPitch)
            {
                soundEmitter.WithRandomPitch();
            }

            if (soundData.frequentSound)
            {
                soundEmitter.Node = _soundManager.GetFrequentSoundEmitters().AddLast(soundEmitter);
            }

            soundEmitter.Play();
        }
    }
}