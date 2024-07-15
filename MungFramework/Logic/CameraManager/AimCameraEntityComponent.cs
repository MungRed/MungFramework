using MungFramework.Entity;
/// <summary>
/// ���ڶ����ϼ���ʵ�ֿ��������
/// </summary>
namespace MungFramework.Logic.Camera
{
    public class AimCameraEntityComponent : EntityComponent
    {
        public void OnEnable()
        {
            CameraManagerAbstract.Instance.AimCameraController.Add(this);
        }
        public void OnDisable()
        {
            CameraManagerAbstract.Instance?.AimCameraController.Remove(this);
        }
    }
}

