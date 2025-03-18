using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace MungFramework.Logic.Sound
{
    [Serializable]
    public class SoundSource : Model.Model
    {
        public SoundSource(string id, Transform follow, Vector3 localPosition, AudioSource source, VolumeTypeEnum volumeType, float volume)
        {
            Id = id;
            Follow = follow;
            LocalPosition = localPosition;
            Source = source;
            VolumeType = volumeType;
            Volume = volume;
        }

        [ShowInInspector]
        public string Id
        {
            get;
        }

        [ShowInInspector]
        public Transform Follow
        {
            get;
            set;
        }

        [ShowInInspector]
        public Vector3 LocalPosition
        {
            get;
            set;
        }

        [ShowInInspector]
        public AudioSource Source
        {
            get;
            set;
        }

        [ShowInInspector]
        public VolumeTypeEnum VolumeType
        {
            get;
        }

        [ShowInInspector]
        public float Volume
        {
            get;
            set;
        }

    }
}


