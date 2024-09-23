/// <summary>
/// 挂在对象上即可实现看向摄像机
/// </summary>
namespace MungFramework.Logic.Camera
{
    public class AimCameraEntity : MungFramework.Entity.Entity
    {
        public void OnEnable()
        {
            CameraManagerAbstract.Instance.AimCameraController.AddAimCameraEntity(this);
        }
        public void OnDisable()
        {
            CameraManagerAbstract.Instance?.AimCameraController.RemoveAimCamerEntity(this);
        }
    }
}

