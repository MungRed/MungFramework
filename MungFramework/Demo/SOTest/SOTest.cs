using MungFramework.ScriptableObjects;
using UnityEngine;



namespace MungFramework.Demo
{
    [CreateAssetMenu(fileName = "newso", menuName = "SO/SOTest")]
    public class SOTest : DataSO
    {
        public string Name;
        public string Description;
    }
}

