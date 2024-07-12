using System.Collections;
using UnityEngine;

namespace MungFramework.Logic
{
    public class SingletonGameManagerAbstract<T> : GameManagerAbstract where T: GameManagerAbstract
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