using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace MungFramework.Logic.Camera
{
    public class AimCameraControllerAbstarct : GameManagerAbstract
    {
        private UnityEngine.Camera mainCamera => UnityEngine.Camera.main;

        [ReadOnly]
        [SerializeField]
        private List<AimCameraEntity> needAimCameraList = new();

        [SerializeField]
        [Required("��Ҫ����")]
        private Transform directionTransform;


        public void AddAimCameraEntity(AimCameraEntity aimCamera)
        {
            if (needAimCameraList.Contains(aimCamera))
            {
                return;
            }

            needAimCameraList.Add(aimCamera);
            aimCamera.transform.rotation = mainCamera.transform.rotation;
        }
        public void RemoveAimCamerEntity(AimCameraEntity aimCamera)
        {
            needAimCameraList.Remove(aimCamera);
        }

        public override void OnGameUpdate(GameManagerAbstract parentManager)
        {
            base.OnGameUpdate(parentManager);

            //���·���
            directionTransform.eulerAngles = new Vector3(0f, mainCamera.transform.eulerAngles.y, mainCamera.transform.eulerAngles.z);

            //����ÿ����Ҫ���������������
            foreach (var aimCamera in needAimCameraList)
            {
                aimCamera.transform.rotation = mainCamera.transform.rotation;
            }
        }


        /// <summary>
        /// ��������������������������������
        /// </summary>
        public Vector3 CameraToWorld(Vector3 input) => directionTransform.TransformDirection(input);

        /// <summary>
        /// ���������������������ػ��������������
        /// </summary>
        public Vector3 WolrdToCamera(Vector3 input) => directionTransform.InverseTransformDirection(input);

        /// <summary>
        /// �ж�ĳ�������Ƿ�����Ұ��
        /// </summary>
        public bool IsInView(Vector3 worldPos)
        {
            Transform camTransform = mainCamera.transform;
            Vector2 viewPos = mainCamera.WorldToViewportPoint(worldPos);

            //�ж������Ƿ������ǰ��  
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

