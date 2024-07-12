using MungFramework.Logic.Save;
using System;
using System.Collections;
using UnityEngine;

namespace MungFramework.Logic.Sound
{
    public abstract class SoundDataManagerAbstract : GameManagerAbstract
    {
        public enum VolumeTypeEnum
        {
            Music, Effect, Voice
        }

        [Serializable]
        public class VolumeData
        {
            public int MusicVolume;
            public int EffectVolume;
            public int VoiceVolume;
        }

        [SerializeField]
        private VolumeData volumeData = new();

        public override IEnumerator OnSceneLoad(GameManagerAbstract parentManager)
        {
            yield return base.OnSceneLoad(parentManager);
            Load();
        }

        public int GetVolumeData(VolumeTypeEnum volumeType)
        {
            switch (volumeType)
            {
                case VolumeTypeEnum.Music:
                    return volumeData.MusicVolume;
                case VolumeTypeEnum.Effect:
                    return volumeData.EffectVolume;
                case VolumeTypeEnum.Voice:
                    return volumeData.VoiceVolume;
            }
            return 0;
        }
        public void SetVolumeData(VolumeTypeEnum volumeType,int val)
        {
            switch (volumeType)
            {
                case VolumeTypeEnum.Music:
                    volumeData.MusicVolume = val;
                    break;
                case VolumeTypeEnum.Effect:
                    volumeData.EffectVolume = val;
                    break;
                case VolumeTypeEnum.Voice:
                    volumeData.VoiceVolume = val;
                    break;
            }
            Save();
        }


        public void Load()
        {
            var loadSuccess = SaveManagerAbstract.Instance.GetSystemValue("volumedata");
            if (loadSuccess.hasValue == false)
            {
                volumeData = new()
                {
                    MusicVolume = 70,
                    EffectVolume = 70,
                    VoiceVolume = 70
                };
                Save();
            }
            else
            {
                volumeData = JsonUtility.FromJson<VolumeData>(loadSuccess.value);
            }
        }

        public void  Save()
        {
            SaveManagerAbstract.Instance.SetSystemValue("volumedata", JsonUtility.ToJson(volumeData));
        }
    }
}