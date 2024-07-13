using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace MungFramework.Logic.Camera
{
    public class AimCameraControllerAbstarct : GameExecutorAbstract
    {

        private UnityEngine.Camera mainCamera=>UnityEngine.Camera.main;

        [ReadOnly]
        [SerializeField]
        private List<Transform> needAimCameraList = new List<Transform>();

        [SerializeField]
        [Required("需要挂载")]
        private Transform directionTransform;


        public void Add(Transform trans)
        {
            if (needAimCameraList.Contains(trans))
            {
                return;
            }

            needAimCameraList.Add(trans);
        }
        public void Remove(Transform trans)
        {
            needAimCameraList.Remove(trans);
        }

        public override void OnGameUpdate(GameManagerAbstract parentManager)
        {
            base.OnGameUpdate(parentManager);

            //更新方向
            directionTransform.eulerAngles = new Vector3(0f, mainCamera.transform.eulerAngles.y, mainCamera.transform.eulerAngles.z);

            //更新每个需要朝向摄像机的物体
            foreach (Transform t in needAimCameraList)
            {
                t.rotation = mainCamera.transform.rotation;
            }
        }

        /// <summary>
        /// 输入摄像机的相对向量，返回世界向量
        /// </summary>
        public Vector3 CameraToWorld(Vector3 input)
        {
            return directionTransform.TransformDirection(input);
        }

        /// <summary>
        /// 输入基于世界的向量，返回基于摄像机的向量
        /// </summary>
        public Vector3 WolrdToCamera(Vector3 input)
        {
            return directionTransform.InverseTransformDirection(input);
        }

        public bool IsInView(Vector3 worldPos)
        {
            Transform camTransform = mainCamera.transform;
            Vector2 viewPos = mainCamera.WorldToViewportPoint(worldPos);

            //判断物体是否在相机前面  
            Vector3 dir = (worldPos - camTransform.position).normalized;
            float dot = Vector3.Dot(camTransform.forward, dir);

            if (dot > 0 && viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

