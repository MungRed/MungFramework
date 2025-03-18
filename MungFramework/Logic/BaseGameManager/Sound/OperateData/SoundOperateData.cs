using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace MungFramework.Logic.Sound
{
    [Serializable]
    public class SoundOperateData : MungFramework.ModelData.ModelData
    {
        public enum SoundOperateTypeEnum
        {
            PlayAudio = 0,
            AddSoundSouce = 1,
            RemoveSoundSource = 2,
            PauseAudio = 3,
            ResumeAudio = 4,
            StopAudio = 5,
        }

        [SerializeField]
        [LabelText("事件类型")]
        private SoundOperateTypeEnum operateType;

        [SerializeField]
        private bool useDefaultSoundSource = true;
        [SerializeField]
        [ShowIf("showVolumeType")]
        private VolumeTypeEnum volumeType;
        [SerializeField]
        [ShowIf("showSoundSourceId")]
        private string soundSourceId;

        [SerializeField]
        [ShowIf("OperateType", SoundOperateTypeEnum.PlayAudio)]
        private PlayAudioData playAudioData;


        private bool showVolumeType => OperateType == SoundOperateTypeEnum.AddSoundSouce || useDefaultSoundSource;
        private bool showSoundSourceId => OperateType == SoundOperateTypeEnum.AddSoundSouce || !useDefaultSoundSource;

        public SoundOperateTypeEnum OperateType => operateType;
        public bool UseDefaultSoundSource => useDefaultSoundSource;
        public VolumeTypeEnum VolumeType => volumeType;
        public string SoundSourceId => soundSourceId;
        public PlayAudioData PlayAudioData => playAudioData;
    }
}
