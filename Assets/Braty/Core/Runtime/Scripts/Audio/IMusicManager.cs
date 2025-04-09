using UnityEngine;

namespace Braty.Core.Runtime.Scripts.Audio
{
    public interface IMusicManager
    {
        void AddToPlaylist(AudioClip clip);
        void Clear();
        void PlayNextTrack();
        void Play(AudioClip clip);
    }
}