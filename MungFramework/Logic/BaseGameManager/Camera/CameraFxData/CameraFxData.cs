using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace MungFramework.Logic.Camera
{
    [Serializable]
    public class CameraFxData : MungFramework.ModelData.ModelData
    {
        [Serializable]
        public enum CameraFxTypeEnum
        {
            SetNoise = 0,
            ResetNoise = 1,

            SetFov = 10,
            ReseFov= 11,

            AddScreenFx = 20,
            RemoveScreenFx = 21,
        }

        [SerializeField]
        private CameraFxTypeEnum fxType;

        [SerializeField]
        [ShowIf("IsNoise")]
        private SetNoiseData noiseData;

        [SerializeField]
        [ShowIf("IsFov")]
        private SetFovData fovData;

        [SerializeField]
        [ShowIf("IsScreenFx")]
        private ScreenFxData screenFxData;


        private bool IsNoise => FxType == CameraFxTypeEnum.SetNoise || FxType == CameraFxTypeEnum.ResetNoise;
        private bool IsFov => FxType == CameraFxTypeEnum.SetFov || FxType == CameraFxTypeEnum.ReseFov;
        private bool IsScreenFx => FxType == CameraFxTypeEnum.AddScreenFx || FxType == CameraFxTypeEnum.RemoveScreenFx;

        public CameraFxTypeEnum FxType=>fxType;

        public SetNoiseData NoiseData => noiseData;
        public SetFovData FovData => fovData;
        public ScreenFxData ScreenFxData => screenFxData;
    }
}
