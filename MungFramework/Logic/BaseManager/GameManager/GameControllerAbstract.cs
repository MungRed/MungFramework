using Sirenix.OdinInspector;
using UnityEngine;

namespace MungFramework.Logic
{
    /// <summary>
    /// 游戏控制器，游戏管理器树形结构的根节点，只能被游戏管理器调用
    /// </summary>
    public abstract class GameControllerAbstract : MonoBehaviour
    {

        public virtual void OnSceneLoad(GameManagerAbstract parentManager)
        {
            RegisterEventOnSceneLoad();
        }
        protected virtual void RegisterEventOnSceneLoad()
        {

        }
        public virtual void OnGameStart(GameManagerAbstract parentManager)
        {

        }
        public virtual void OnGamePause(GameManagerAbstract parentManager)
        {

        }
        public virtual void OnGameResume(GameManagerAbstract parentManager)
        {

        }
        public virtual void OnGameReload(GameManagerAbstract parentManager)
        {

        }
        public virtual void OnGameReloadFinish(GameManagerAbstract parentManager)
        {

        }
        public virtual void OnGameQuit(GameManagerAbstract parentManager)
        {

        }
        public virtual void OnGameUpdate(GameManagerAbstract parentManager)
        {

        }
        public virtual void OnGameFixedUpdate(GameManagerAbstract parentManager)
        {

        }
        public virtual void OnGameLateUpdate(GameManagerAbstract parentManager)
        {

        }
    }

}
