using UnityEngine;

namespace MungFramework.Logic.Sound
{
    /// <summary>
    /// �Զ���ƵԴ
    /// ������ʱ�Զ������ƵԴ���ڹر�ʱ�Զ��Ƴ���ƵԴ
    /// </summary>
    public class AutoSoundSourceEntity : Entity.Entity
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

