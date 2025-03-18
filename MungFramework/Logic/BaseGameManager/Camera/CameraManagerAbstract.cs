using MungFramework.Logic.TimeCounter;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MungFramework.Logic.Camera
{
    public class CameraManagerAbstract : SingletonGameManagerAbstract<CameraManagerAbstract>
    {
        [Required("需要挂载AimCameraController")]
        public AimCameraControllerAbstarct AimCameraController;

        [Required("需要挂载CameraController")]
        public CameraControllerAbstract CameraController;

        [SerializeField]
        private RectTransform screenFxCanvas;

        [ShowInInspector]
        private Dictionary<string, GameObject> screenFxDic { get; } = new();


        /// <summary>
        /// 输入摄像机的相对向量，返回世界向量
        /// </summary>
        public Vector3 CameraToWorld(Vector3 input) => AimCameraController.CameraToWorld(input);

        /// <summary>
        /// 输入基于世界的向量，返回基于摄像机的向量
        /// </summary>
        public Vector3 WolrdToCamera(Vector3 input) => AimCameraController.WolrdToCamera(input);

        /// <summary>
        /// 判断坐标是否在摄像机视野内
        /// </summary>
        public bool IsInView(Vector3 worldPos) => AimCameraController.IsInView(worldPos);


        public void DoCameraFx(CameraFxData cameraFxData, UnityAction endCallback = null)
        {
            switch (cameraFxData.FxType)
            {
                case CameraFxData.CameraFxTypeEnum.SetNoise:
                    SetNoise(cameraFxData.NoiseData.Value, cameraFxData.NoiseData.Duration, endCallback);
                    return;
                case CameraFxData.CameraFxTypeEnum.ResetNoise:
                    ResetNoise(cameraFxData.NoiseData.Duration, endCallback);
                    return;
                case CameraFxData.CameraFxTypeEnum.SetFov:
                    SetFov(cameraFxData.FovData.Value, cameraFxData.FovData.Duration, endCallback);
                    return;
                case CameraFxData.CameraFxTypeEnum.ReseFov:
                    ResetFov(cameraFxData.FovData.Duration, endCallback);
                    return;
                case CameraFxData.CameraFxTypeEnum.AddScreenFx:
                    AddScreenFx(cameraFxData.ScreenFxData.FxId, cameraFxData.ScreenFxData.FxPrefab);
                    endCallback?.Invoke();
                    if (cameraFxData.ScreenFxData.AutoRemove)
                    {
                        TimeCounterManager.StartTimeCounter(cameraFxData.ScreenFxData.AutoRemoveTime, 0, null, () => RemoveScreenFx(cameraFxData.ScreenFxData.FxId));
                    }
                    return;
                case CameraFxData.CameraFxTypeEnum.RemoveScreenFx:
                    RemoveScreenFx(cameraFxData.ScreenFxData.FxId);
                    endCallback?.Invoke();
                    break;
            }
            endCallback?.Invoke();
        }
        public void ResetAll()
        {
            ClearScreenFx();
            ResetNoise(0, null);
            ResetFov(0, null);
        }

        public void AddScreenFx(string id, GameObject screenFxPrefab)
        {
            if (!screenFxDic.ContainsKey(id)&&screenFxPrefab!=null)
            {
                var obj = Instantiate(screenFxPrefab, screenFxCanvas);
                screenFxDic.Add(id, obj);
            }
        }
        public void RemoveScreenFx(string id)
        {
            if (screenFxDic.ContainsKey(id))
            {
                Destroy(screenFxDic[id]);
                screenFxDic.Remove(id);
            }
        }
        public void ClearScreenFx()
        {
            foreach (var screenFx in screenFxDic)
            {
                Destroy(screenFx.Value);
            }
            screenFxDic.Clear();
        }

        public void SetNoise(float value) => CameraController.SetNoise(value);
        public void SetNoise(float value, float time, UnityAction endCallback = null) => CameraController.SetNoise(value, time, endCallback);
        public void SetNoise(float init, float target, float duration, UnityAction endCallback = null) => CameraController.SetNoise(init, target, duration, endCallback);
        public void ResetNoise(float duration, UnityAction endCallback = null) => CameraController.ResetNoise(duration, endCallback);

        public void SetFov(float val, float time, UnityAction endCallback = null) => CameraController.SetFov(val, time, endCallback);
        public void ResetFov(float time, UnityAction endCallback = null) => CameraController.ResetFov(time, endCallback);

        public CameraSource GetCameraSource() => CameraController.GetCameraSource();

        public IEnumerator ChangeCameraSource(CameraSource cameraSource, float time) => CameraController.ChangeCameraSource(cameraSource, time);
        public IEnumerator ChangeBindFollow(Transform aim, float time) => CameraController.ChangeBindFollow(aim, time);
        public IEnumerator ChangeBindLookAt(Transform aim, float time) => CameraController.ChangeBindLookAt(aim, time);

#if UNITY_EDITOR
        [Button]
        public void ResetCamera()
        {
            CameraController.ResetCamera();
        }
        [Button]
        public void MoveCamera(Vector3 followPos, Vector3 lookAtPos)
        {
            CameraController.MoveCamera(followPos, lookAtPos);
        }
#endif

    }
}