using UnityEngine;


namespace MungFramework.Extension.MonoBehaviourExtension
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _Instance;
        public static T Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = FindObjectOfType(typeof(T)) as T;
                }
                return _Instance;
            }
        }
    }

}

