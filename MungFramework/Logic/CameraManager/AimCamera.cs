using UnityEngine;

/// <summary>
/// ���ڶ����ϼ���ʵ�ֿ��������
/// </summary>
namespace MungFramework.Logic.Camera
{
    public class AimCamera : MonoBehaviour
    {
        public void OnEnable()
        {
            CameraManagerAbstract.Instance.AimCameraController.Add(transform);
        }
        public void OnDisable()
        {
            CameraManagerAbstract.Instance?.AimCameraController.Remove(transform);
        }
    }
}

