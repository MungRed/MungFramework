using System;
using UnityEngine;

namespace MungFramework.Logic.Camera
{
    [Serializable]
    public class CameraSource : Model.Model
    {
        public Transform Follow;
        public Transform LookAt;

        public CameraSource(Transform follow, Transform lookAt)
        {
            Follow = follow;
            LookAt = lookAt;
        }
    }
}