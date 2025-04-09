using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Braty.Core.Runtime.Scripts.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicManager : MonoBehaviour, IMusicManager
    {
        [SerializeField] private List<AudioClip> _initialPlaylist;
        [SerializeField] private AudioMixerGroup _musicMixerGroup;

        private const float CrossFadeTime = 1.0f;
        private float _fading;
        private AudioSource _current;
        private AudioSource _previous;
        private readonly Queue<AudioClip> _playlist = new();

        private void Awake()
        {
            _current = GetComponent<AudioSource>();
            foreach (var clip in _initialPlaylist)
            {
                AddToPlayListInternal(clip);
            }
        }

        void IMusicManager.AddToPlaylist(AudioClip clip)
        {
            AddToPlayListInternal(clip);
        }

        private void AddToPlayListInternal(AudioClip clip)
        {
            _playlist.Enqueue(clip);
            if (_current == null && _previous == null)
            {
                PlayNextTrackInternal();
            }
        }

        void IMusicManager.Clear() => _playlist.Clear();

        void IMusicManager.PlayNextTrack()
        {
            PlayNextTrackInternal();
        }

        private void PlayNextTrackInternal()
        {
            if (_playlist.TryDequeue(out AudioClip nextTrack))
            {
                PlayInternal(nextTrack);
            }
        }

        void IMusicManager.Play(AudioClip clip)
        {
            PlayInternal(clip);
        }

        private void PlayInternal(AudioClip clip)
        {
            if (_current && _current.clip == clip) return;

            if (_previous)
            {
                Destroy(_previous);
                _previous = null;
            }

            _previous = _current;

            _current.clip = clip;
            _current.outputAudioMixerGroup = _musicMixerGroup; // Set mixer group
            _current.loop = false; // For playlist functionality, we want tracks to play once
            _current.volume = 0;
            _current.bypassListenerEffects = true;
            _current.Play();

            _fading = 0.001f;
        }

        private void Update()
        {
            HandleCrossFade();

            if (_current && !_current.isPlaying && _playlist.Count > 0)
            {
                PlayNextTrackInternal();
            }
        }

        private void HandleCrossFade()
        {
            if (_fading <= 0f) return;

            _fading += Time.deltaTime;

            float fraction = Mathf.Clamp01(_fading / CrossFadeTime);

            // Logarithmic fade
            float logFraction = fraction.ToLogarithmicFraction();

            if (_previous) _previous.volume = 1.0f - logFraction;
            if (_current) _current.volume = logFraction;

            if (fraction >= 1)
            {
                _fading = 0.0f;
                if (_previous)
                {
                    Destroy(_previous);
                    _previous = null;
                }
            }
        }
    }
}