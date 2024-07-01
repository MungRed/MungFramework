using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace MungFramework.Logic
{
    [Serializable]
    public class GameManagerEvents
    {
        public enum GameMangerEventsEnum
        {
            OnSceneLoad,OnGameStart,
            OnGamePause,OnGameResume,
            OnGameUpdate,OnGameFixedUpdate
        }

        [SerializeField]
        private SerializedDictionary<GameMangerEventsEnum, UnityEvent> GameManagerEventMap;


        public UnityEvent GetEvent(GameMangerEventsEnum gameManagerEventsEnum)
        {
            if (GameManagerEventMap.ContainsKey(gameManagerEventsEnum))
            {
                return GameManagerEventMap[gameManagerEventsEnum];
            }
            else
            {
                return null;
            }
        }
        public void AddEvent(GameMangerEventsEnum gameMangerEventsEnum, UnityAction action)
        {
            if (!GameManagerEventMap.ContainsKey(gameMangerEventsEnum))
            {
                GameManagerEventMap.Add(gameMangerEventsEnum, new UnityEvent());
            }

            GameManagerEventMap[gameMangerEventsEnum].AddListener(action);
        }

        public void RemoveEvent(GameMangerEventsEnum gameMangerEventsEnum, UnityAction action)
        {
            if (GameManagerEventMap.ContainsKey(gameMangerEventsEnum))
            {
                GameManagerEventMap[gameMangerEventsEnum].RemoveListener(action);
            }
        }
    }
}
