using UnityEngine;

namespace MungFramework.Logic
{
    /// <summary>
    /// 游戏执行器，只能被游戏管理器调用
    /// </summary>
    public abstract class GameExecutor : MonoBehaviour
    {
        public virtual void OnSceneLoad(GameManager parentManager)
        {
        }
        public virtual void OnGameStart(GameManager parentManager)
        {
        }
        public virtual void OnGameUpdate(GameManager parentManager)
        {
        }
        public virtual void OnGameFixedUpdate(GameManager parentManager)
        {
        }
        public virtual void OnGamePause(GameManager parentManager)
        {
        }
        public virtual void OnGameResume(GameManager parentManager)
        {
        }
    }

}
