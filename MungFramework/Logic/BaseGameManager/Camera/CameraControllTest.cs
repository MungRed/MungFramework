using Sirenix.OdinInspector;
using UnityEngine;


namespace MungFramework.Logic.Camera
{
    public class CameraControllTest : MonoBehaviour
    {
        public CameraSource CameraSource1, CameraSource2;

        public float time;



        [Button("ChangeBind1", ButtonSizes.Medium)]
        public void ChangeBind1()
        {
            StartCoroutine(CameraManagerAbstract.Instance.ChangeCameraSource(CameraSource1, time));
        }

        [Button("ChangeBind2", ButtonSizes.Medium)]
        public void ChanageBind2()
        {
            StartCoroutine(CameraManagerAbstract.Instance.ChangeCameraSource(CameraSource2, time));
        }

        [Button("Pause", ButtonSizes.Medium)]
        public void Pause()
        {
            CameraManagerAbstract.Instance.OnGamePause(null);
        }

        [Button("Resume", ButtonSizes.Medium)]
        public void Resume()
        {
            CameraManagerAbstract.Instance.OnGameResume(null);
        }

    }



}

