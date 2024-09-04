/// <summary>
/// ���ڶ����ϼ���ʵ�ֿ��������
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

