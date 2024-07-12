using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MungFramework.Logic.Sound
{
    public class AutoSoundSource : MonoBehaviour
    {
        [SerializeField]
        private string soundSourceId;
        [SerializeField]
        private Transform soundSourceFollow;
        [SerializeField]
        private Vector3 soundSourceLocalPosition;

        private void OnEnable()
        {
            SoundManagerAbstract.Instance.AddSoundSource(soundSourceId, SoundDataManagerAbstract.VolumeTypeEnum.Music)
                .SetSoundSourceLocalPosition(soundSourceId,soundSourceLocalPosition);
            if (soundSourceFollow != null)
            {
                SoundManagerAbstract.Instance.SetSoundeSourceFollow(soundSourceId, soundSourceFollow);
            }
        }
        private void OnDisable()
        {
            SoundManagerAbstract.Instance?.RemoveSoundSource(soundSourceId);
        }
    }
}

