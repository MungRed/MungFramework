using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace MungFramework.Logic.Camera
{
    [Serializable]
    public class ScreenFxData : MungFramework.ModelData.ModelData
    {
        [SerializeField]
        private string fxId;
        [SerializeField]
        private GameObject fxPrefab;
        [SerializeField]
        private bool autoRemove = false;
        [SerializeField]
        [ShowIf("AutoRemove")]
        private float autoRemoveTime;


        public string FxId => fxId;
        public GameObject FxPrefab => fxPrefab;
        public bool AutoRemove => autoRemove;
        public float AutoRemoveTime => autoRemoveTime;
    }
}
