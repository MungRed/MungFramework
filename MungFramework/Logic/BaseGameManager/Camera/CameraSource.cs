using System;
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

        //在初始化时记录初始位置
        public Vector3 FollowOriPosition
        {
            get;
        }
        public Vector3 LookAtOriPosition
        {
            get;
        }

        public CameraSource(Transform follow, Transform lookAt)
        {
            this.follow = follow;
            this.lookAt = lookAt;
            FollowOriPosition = follow.position;
            LookAtOriPosition = lookAt.position;
        }
    }
}