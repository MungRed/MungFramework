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
            OnGameReload,OnGameReloadFinish,OnGameQuit,
            OnGameUpdate,OnGameFixedUpdate,
            OnGameLateUpdate
        }

        [SerializeField]
        private SerializedDictionary<GameMangerEventsEnum, UnityEvent> gameManagerEventMap;


        public UnityEvent GetEvent(GameMangerEventsEnum gameManagerEventsEnum)
        {
            if (gameManagerEventMap.ContainsKey(gameManagerEventsEnum))
            {
                return gameManagerEventMap[gameManagerEventsEnum];
            }
            else
            {
                return null;
            }
        }
        public void AddEvent(GameMangerEventsEnum gameMangerEventsEnum, UnityAction action)
        {
            if (!gameManagerEventMap.ContainsKey(gameMangerEventsEnum))
            {
                gameManagerEventMap.Add(gameMangerEventsEnum, new UnityEvent());
            }

            gameManagerEventMap[gameMangerEventsEnum].AddListener(action);
        }

        public void RemoveEvent(GameMangerEventsEnum gameMangerEventsEnum, UnityAction action)
        {
            if (gameManagerEventMap.ContainsKey(gameMangerEventsEnum))
            {
                gameManagerEventMap[gameMangerEventsEnum].RemoveListener(action);
            }
        }
    }
}
