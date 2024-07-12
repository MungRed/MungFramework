using System.Collections;
using UnityEngine;

namespace MungFramework.Logic
{
    public class SingletonGameManagerAbstract<T> : GameManagerAbstract where T: GameManagerAbstract
    {
        public static T Instance;
        public  virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
            }
            else
            {
                Debug.LogError("单例已存在");
                Destroy(gameObject);
            }
        }
    }
}