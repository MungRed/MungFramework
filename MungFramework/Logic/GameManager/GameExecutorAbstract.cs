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
        protected GameManagerEvents gameExecutorEvents = new();


        public virtual IEnumerator OnSceneLoad(GameManagerAbstract parentManager)
        {
            gameExecutorEvents.GetEvent(GameManagerEvents.GameMangerEventsEnum.OnSceneLoad)?.Invoke();
            yield return null;
        }

        public virtual IEnumerator OnGameStart(GameManagerAbstract parentManager)
        {
            gameExecutorEvents.GetEvent(GameManagerEvents.GameMangerEventsEnum.OnGameStart)?.Invoke();
            yield return null;
        }
        public virtual IEnumerator OnGamePause(GameManagerAbstract parentManager)
        {
            gameExecutorEvents.GetEvent(GameManagerEvents.GameMangerEventsEnum.OnGamePause)?.Invoke();
            yield return null;
        }
        public virtual IEnumerator OnGameResume(GameManagerAbstract parentManager)
        {
            gameExecutorEvents.GetEvent(GameManagerEvents.GameMangerEventsEnum.OnGameResume)?.Invoke();
            yield return null;
        }

        public virtual void OnGameUpdate(GameManagerAbstract parentManager)
        {
            gameExecutorEvents.GetEvent(GameManagerEvents.GameMangerEventsEnum.OnGameUpdate)?.Invoke();
        }
        public virtual void OnGameFixedUpdate(GameManagerAbstract parentManager)
        {
            gameExecutorEvents.GetEvent(GameManagerEvents.GameMangerEventsEnum.OnGameFixedUpdate)?.Invoke();
        }
    }

}
