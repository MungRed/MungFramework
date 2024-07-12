using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace MungFramework.Logic
{
    /// <summary>
    /// 游戏执行器，只能被游戏管理器调用
    /// </summary>
    public abstract class GameExecutorAbstract : MonoBehaviour
    {

        [SerializeField]
        [FoldoutGroup("事件")]
        [LabelText("事件")]
        protected GameManagerEvents GameExecutorEvents = new();


        public virtual IEnumerator OnSceneLoad(GameManagerAbstract parentManager)
        {
            GameExecutorEvents.GetEvent(GameManagerEvents.GameMangerEventsEnum.OnSceneLoad)?.Invoke();
            yield return null;
        }

        public virtual IEnumerator OnGameStart(GameManagerAbstract parentManager)
        {
            GameExecutorEvents.GetEvent(GameManagerEvents.GameMangerEventsEnum.OnGameStart)?.Invoke();
            yield return null;
        }
        public virtual IEnumerator OnGamePause(GameManagerAbstract parentManager)
        {
            GameExecutorEvents.GetEvent(GameManagerEvents.GameMangerEventsEnum.OnGamePause)?.Invoke();
            yield return null;
        }
        public virtual IEnumerator OnGameResume(GameManagerAbstract parentManager)
        {
            GameExecutorEvents.GetEvent(GameManagerEvents.GameMangerEventsEnum.OnGameResume)?.Invoke();
            yield return null;
        }

        public virtual void OnGameUpdate(GameManagerAbstract parentManager)
        {
            GameExecutorEvents.GetEvent(GameManagerEvents.GameMangerEventsEnum.OnGameUpdate)?.Invoke();
        }
        public virtual void OnGameFixedUpdate(GameManagerAbstract parentManager)
        {
            GameExecutorEvents.GetEvent(GameManagerEvents.GameMangerEventsEnum.OnGameFixedUpdate)?.Invoke();
        }
    }

}
