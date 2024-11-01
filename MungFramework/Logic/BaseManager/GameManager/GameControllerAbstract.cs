using Sirenix.OdinInspector;
using UnityEngine;

namespace MungFramework.Logic
{
    /// <summary>
    /// 游戏控制器，游戏管理器树形结构的根节点，只能被游戏管理器调用
    /// </summary>
    public abstract class GameControllerAbstract : MonoBehaviour
    {

        [SerializeField]
        [FoldoutGroup("事件")]
        [LabelText("事件")]
        protected GameManagerEvents gameControllerEvents = new();


        public virtual void OnSceneLoad(GameManagerAbstract parentManager)
        {
            RegisterEventOnSceneLoad();
            gameControllerEvents.GetEvent(GameManagerEvents.GameMangerEventsEnum.OnSceneLoad)?.Invoke();
        }
        protected virtual void RegisterEventOnSceneLoad()
        {

        }

        public virtual void OnGameStart(GameManagerAbstract parentManager)
        {
            gameControllerEvents.GetEvent(GameManagerEvents.GameMangerEventsEnum.OnGameStart)?.Invoke();
        }
        public virtual void OnGamePause(GameManagerAbstract parentManager)
        {
            gameControllerEvents.GetEvent(GameManagerEvents.GameMangerEventsEnum.OnGamePause)?.Invoke();
        }
        public virtual void OnGameResume(GameManagerAbstract parentManager)
        {
            gameControllerEvents.GetEvent(GameManagerEvents.GameMangerEventsEnum.OnGameResume)?.Invoke();
        }

        public virtual void OnGameReload(GameManagerAbstract parentManager)
        {
            gameControllerEvents.GetEvent(GameManagerEvents.GameMangerEventsEnum.OnGameReload)?.Invoke();
        }
        public virtual void OnGameReloadFinish(GameManagerAbstract parentManager)
        {
            gameControllerEvents.GetEvent(GameManagerEvents.GameMangerEventsEnum.OnGameReloadFinish)?.Invoke();
        }

        public virtual void OnGameQuit(GameManagerAbstract parentManager)
        {
            gameControllerEvents.GetEvent(GameManagerEvents.GameMangerEventsEnum.OnGameQuit)?.Invoke();
        }

        public virtual void OnGameUpdate(GameManagerAbstract parentManager)
        {
            gameControllerEvents.GetEvent(GameManagerEvents.GameMangerEventsEnum.OnGameUpdate)?.Invoke();
        }
        public virtual void OnGameFixedUpdate(GameManagerAbstract parentManager)
        {
            gameControllerEvents.GetEvent(GameManagerEvents.GameMangerEventsEnum.OnGameFixedUpdate)?.Invoke();
        }
        public virtual void OnGameLateUpdate(GameManagerAbstract parentManager)
        {
            gameControllerEvents.GetEvent(GameManagerEvents.GameMangerEventsEnum.OnGameLateUpdate)?.Invoke();
        }
    }

}
