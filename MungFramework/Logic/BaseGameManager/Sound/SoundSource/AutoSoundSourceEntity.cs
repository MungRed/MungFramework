using UnityEngine;

namespace MungFramework.Logic.Sound
{
    /// <summary>
    /// 自动音频源
    /// 在启动时自动添加音频源，在关闭时自动移除音频源
    /// </summary>
    public class AutoSoundSourceEntity : Entity.Entity
    {
        [SerializeField]
        private VolumeTypeEnum volumeType;

        [SerializeField]
        private string soundSourceId;
        [SerializeField]
        private Transform soundSourceFollow;
        [SerializeField]
        private Vector3 soundSourceLocalPosition;

        private void OnEnable()
        {
            SoundManagerAbstract.Instance.AddSoundSource(soundSourceId, volumeType)
                .SetSoundSourceLocalPosition(soundSourceId, soundSourceLocalPosition);
            if (soundSourceFollow != null)
            {
                SoundManagerAbstract.Instance.SetSoundSourceFollow(soundSourceId, soundSourceFollow);
            }
        }
        private void OnDisable()
        {
            SoundManagerAbstract.Instance?.RemoveSoundSource(soundSourceId);
        }
    }
}

