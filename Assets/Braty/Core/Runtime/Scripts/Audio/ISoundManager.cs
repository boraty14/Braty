using System.Collections.Generic;
using UnityEngine;

namespace Braty.Core.Runtime.Scripts.Audio
{
    public interface ISoundManager
    {
        SoundBuilder CreateSoundBuilder();
        bool CanPlaySound(SoundData data);
        SoundEmitter Get();
        void ReturnToPool(SoundEmitter soundEmitter);
        void StopAll();
        LinkedListNode<SoundEmitter> AddFrequentSoundEmitter(SoundEmitter soundEmitter);
        Transform GetSoundTransform();
    }
}