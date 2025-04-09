using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Braty.Core.Runtime.Scripts.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundEmitter : MonoBehaviour
    {
        public SoundData Data { get; private set; }
        public LinkedListNode<SoundEmitter> Node { get; set; }

        private ISoundManager _soundManager;
        private AudioSource _audioSource;
        private Coroutine _playingCoroutine;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void Initialize(SoundData data, ISoundManager soundManager)
        {
            _soundManager = soundManager;
            
            Data = data;
            _audioSource.clip = data.clip;
            _audioSource.outputAudioMixerGroup = data.mixerGroup;
            _audioSource.loop = data.loop;
            _audioSource.playOnAwake = data.playOnAwake;

            _audioSource.mute = data.mute;
            _audioSource.bypassEffects = data.bypassEffects;
            _audioSource.bypassListenerEffects = data.bypassListenerEffects;
            _audioSource.bypassReverbZones = data.bypassReverbZones;

            _audioSource.priority = data.priority;
            _audioSource.volume = data.volume;
            _audioSource.pitch = data.pitch;
            _audioSource.panStereo = data.panStereo;
            _audioSource.spatialBlend = data.spatialBlend;
            _audioSource.reverbZoneMix = data.reverbZoneMix;
            _audioSource.dopplerLevel = data.dopplerLevel;
            _audioSource.spread = data.spread;

            _audioSource.minDistance = data.minDistance;
            _audioSource.maxDistance = data.maxDistance;

            _audioSource.ignoreListenerVolume = data.ignoreListenerVolume;
            _audioSource.ignoreListenerPause = data.ignoreListenerPause;

            _audioSource.rolloffMode = data.rolloffMode;
        }

        public void Play()
        {
            if (_playingCoroutine != null)
            {
                StopCoroutine(_playingCoroutine);
            }

            _audioSource.Play();
            _playingCoroutine = StartCoroutine(WaitForSoundToEnd());
        }

        IEnumerator WaitForSoundToEnd()
        {
            yield return new WaitWhile(() => _audioSource.isPlaying);
            Stop();
        }

        public void Stop()
        {
            if (_playingCoroutine != null)
            {
                StopCoroutine(_playingCoroutine);
                _playingCoroutine = null;
            }

            _audioSource.Stop();
            _soundManager.ReturnToPool(this);
        }

        public void WithRandomPitch(float min = -0.05f, float max = 0.05f)
        {
            _audioSource.pitch += Random.Range(min, max);
        }
    }
}