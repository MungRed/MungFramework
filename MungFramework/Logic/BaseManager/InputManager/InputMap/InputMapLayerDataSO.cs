using MungFramework.ScriptableObjects;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MungFramework.Logic.Input
{
    [CreateAssetMenu(fileName = "newInputMapLayerDataSO", menuName = "Mung/Input/InputMapLayerDataSO")]
    [Serializable]
    public class InputMapLayerDataSO : DataSO
    {
        public string InputMapLayerName;

        public List<InputMapKeyValuePair> InputMapList = new();
    }
}