using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Braty.Core.Runtime.Scripts.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicManagerBehaviour : MonoBehaviour
    {
        [SerializeField] private List<AudioClip> _initialPlaylist;
        [SerializeField] private AudioMixerGroup _musicMixerGroup;

        public List<AudioClip> InitialPlaylist => _initialPlaylist;
        public AudioMixerGroup MusicMixerGroup => _musicMixerGroup;
        

        public void DestroyAudioSource(AudioSource audioSource)
        {
            Destroy(audioSource);
        }
    }
}