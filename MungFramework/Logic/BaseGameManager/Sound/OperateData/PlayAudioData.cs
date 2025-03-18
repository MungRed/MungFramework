using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace MungFramework.Logic.Sound
{
    [Serializable]
    public class PlayAudioData : ModelData.ModelData
    {
        [SerializeField]
        private AudioClip audioClip;

        [SerializeField]
        private bool oneShot = true;
        [SerializeField]
        [HideIf("OneShot")]
        private bool loop;
        [SerializeField]
        [HideIf("OneShot")]
        private bool transition;
        [SerializeField]
        [HideIf("OneShot")]
        private bool forceReplace;


        public AudioClip AudioClip => audioClip;
        public bool OneShot => oneShot;
        public bool Loop => loop;
        public bool Transition => transition;
        public bool ForceReplace => forceReplace;
    }
}
