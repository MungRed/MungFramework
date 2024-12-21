using MungFramework.Logic.Save;
using System;
using System.Collections;
using UnityEngine;

namespace MungFramework.Logic.Sound
{
    public abstract class SoundDataManagerAbstract : GameManagerAbstract
    {
        private static readonly string VolumeSaveDataKey = "VolumeSaveData";
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

        public override void OnSceneLoad(GameManagerAbstract parentManager)
        {
            base.OnSceneLoad(parentManager);
            Load();
        }


        internal virtual int GetVolumeData(VolumeTypeEnum volumeType)
        {
            return volumeType switch
            {
                VolumeTypeEnum.Music => volumeData.MusicVolume,
                VolumeTypeEnum.Effect => volumeData.EffectVolume,
                VolumeTypeEnum.Voice => volumeData.VoiceVolume,
                _ => 0,
            };
        }

        internal virtual void SetVolumeData(VolumeTypeEnum volumeType,int val)
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
        internal virtual void DefaultVolumeData()
        {
            volumeData = new()
            {
                MusicVolume = 70,
                EffectVolume = 70,
                VoiceVolume = 70
            };
        }


        protected virtual void Load()
        {
            var loadSuccess = SaveManagerAbstract.Instance.GetSystemSaveValue(VolumeSaveDataKey);
            if (loadSuccess.hasValue)
            {
                JsonUtility.FromJsonOverwrite(loadSuccess.value, volumeData);
            }
            else
            {
                DefaultVolumeData();
                Save();
            }
        }

        protected virtual void  Save()
        {
            SaveManagerAbstract.Instance.SetSystemSaveValue(VolumeSaveDataKey, JsonUtility.ToJson(volumeData));
        }
    }
}