using UnityEngine;

/// <summary>
/// 挂在对象上即可实现看向摄像机
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

