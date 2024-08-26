using Sirenix.OdinInspector;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace MungFramework.Logic.Camera
{
    public class CameraManagerAbstract : SingletonGameManagerAbstract<CameraManagerAbstract>
    {
#if UNITY_EDITOR
        private bool isBind1
        {
            get
            {
                bool res = !(AimCameraController != null && subGameControllerList.Contains(AimCameraController));
                if (res == true)
                {
                    Debug.LogError(name+"需要挂载到子执行器");
                }
                return res;
            }
        }
        private bool isBind2
        {
            get
            {
                bool res =  !(CameraController != null && subGameControllerList.Contains(CameraController));
                if (res == true)
                {
                    Debug.LogError(name + "需要挂载到子执行器");
                }
                return res;
            }
        }
#endif

        [Required("需要挂载AimCameraController")]
        [InfoBox("需要挂载到子执行器","isBind1",InfoMessageType = InfoMessageType.Error)]
        public AimCameraControllerAbstarct AimCameraController;

        [Required("需要挂载CameraController")]
        [InfoBox("需要挂载到子执行器", "isBind2", InfoMessageType = InfoMessageType.Error)]
        public CameraControllerAbstract CameraController;


        /// <summary>
        /// 输入摄像机的相对向量，返回世界向量
        /// </summary>
        public Vector3 CameraToWorld(Vector3 input)
        {
            return AimCameraController.CameraToWorld(input);
        }

        /// <summary>
        /// 输入基于世界的向量，返回基于摄像机的向量
        /// </summary>
        public Vector3 WolrdToCamera(Vector3 input)
        {
            return AimCameraController.WolrdToCamera(input);
        }

        public bool IsInView(Vector3 worldPos)
        {
            return AimCameraController.IsInView(worldPos);
        }


        public void SetFov(int val, float time) => CameraController.SetFov(val, time);
        public void ResetFov(float time) => CameraController.ResetFov(time);

        public CameraSource GetCameraSource() => CameraController.GetCameraSource();

        public IEnumerator ChangeCameraSource(CameraSource cameraSource, float time) => CameraController.ChangeCameraSource(cameraSource, time);
        public IEnumerator ChangeBindFollow(Transform aim, float time) => CameraController.ChangeBindFollow(aim, time);
        public IEnumerator ChangeBindLookAt(Transform aim, float time) => CameraController.ChangeBindLookAt(aim, time);


    }
}