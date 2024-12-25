using Sirenix.OdinInspector;
using System.Collections;
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

        public void SetNoise(float value) => CameraController.SetNoise(value);
        public void SetNoise(float value, float time, UnityAction endCallback = null) => CameraController.SetNoise(value, time, endCallback);
        public void SetNoise(float init, float target, float duration, UnityAction endCallback = null) => CameraController.SetNoise(init, target, duration, endCallback);
        public void ResetNoise() => CameraController.ResetNoise();

        public void SetFov(int val, float time) => CameraController.SetFov(val, time);
        public void ResetFov(float time) => CameraController.ResetFov(time);

        public CameraSource GetCameraSource() => CameraController.GetCameraSource();

        public IEnumerator ChangeCameraSource(CameraSource cameraSource, float time) => CameraController.ChangeCameraSource(cameraSource, time);
        public IEnumerator ChangeBindFollow(Transform aim, float time) => CameraController.ChangeBindFollow(aim, time);
        public IEnumerator ChangeBindLookAt(Transform aim, float time) => CameraController.ChangeBindLookAt(aim, time);


    }
}