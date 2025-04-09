using System.Collections.Generic;
using UnityEngine;

namespace Braty.Core.Runtime.Audio
{
    public interface ISoundManager
    {
        SoundBuilder CreateSoundBuilder();
        bool CanPlaySound(SoundData data);
        SoundEmitter Get();
        void ReturnToPool(SoundEmitter soundEmitter);
        void StopAll();
        LinkedList<SoundEmitter> GetFrequentSoundEmitters();
        Transform GetSoundTransform();
    }
}