using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace MungFramework.Logic.Sound
{
    [Serializable]
    public class PlayAudioData : ModelData.ModelData
    {
        [SerializeField]
        private bool useDefaultSoundSource = true;

        [SerializeField]
        [ShowIf("UseDefaultSoundSource", true)]
        private VolumeTypeEnum volumeType;
        [SerializeField]
        [ShowIf("UseDefaultSoundSource", false)]
        private string soundSourceId;

        [SerializeField]
        private AudioClip audioClip;

        [SerializeField]
        private bool oneShot = true;
        [SerializeField]
        [ShowIf("OneShot", false)]
        private bool loop;
        [SerializeField]
        [ShowIf("OneShot", false)]
        private bool transition;
        [SerializeField]
        [ShowIf("OneShot", false)]
        private bool forceReplace;


        public bool UseDefaultSoundSource => useDefaultSoundSource;
        public VolumeTypeEnum VolumeType => volumeType;
        public string SoundSourceId => soundSourceId;
        public AudioClip AudioClip => audioClip;
        public bool OneShot => oneShot;

        public bool Loop => loop;
        public bool Transition => transition;
        public bool ForceReplace => forceReplace;
    }
}
