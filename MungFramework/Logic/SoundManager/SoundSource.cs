using System;
using UnityEngine;

namespace MungFramework.Logic.Sound
{
    [Serializable]
    public class SoundSource : MungFramework.Model.Model
    {
        public string id;//id
        public Transform Follow;//跟随的对象
        public Vector3 LocalPosition;//与跟随对象的相对位置

        public AudioSource Source;//音频源
        public SoundDataManagerAbstract.VolumeTypeEnum VolumeType;//音量类型
        public float Volume;//音量
    }
}


