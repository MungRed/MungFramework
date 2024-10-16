using System;
using System.Linq;
using UnityEngine;

namespace MungFramework.Logic.Camera
{
    [Serializable]
    public class CameraSource : Model.Model
    {
        [SerializeField]
        private Transform follow;
        [SerializeField]
        private Transform lookAt;

        public Transform Follow => follow;
        public Transform LookAt => lookAt;
        public CameraSource(Transform follow, Transform lookAt)
        {
            this.follow = follow;
            this.lookAt = lookAt;
        }
    }
}