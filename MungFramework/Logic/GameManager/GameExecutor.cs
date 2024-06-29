using System.Collections;
using UnityEngine;

namespace MungFramework.Logic
{
    /// <summary>
    /// 游戏执行器，只能被游戏管理器调用
    /// </summary>
    public abstract class GameExecutor : MonoBehaviour
    {
        public virtual IEnumerator OnSceneLoad(GameManager parentManager)
        {
            yield return null;
        }

        public virtual IEnumerator OnGameStart(GameManager parentManager)
        {
            yield return null;
        }
        public virtual IEnumerator OnGamePause(GameManager parentManager)
        {
            yield return null;
        }
        public virtual IEnumerator OnGameResume(GameManager parentManager)
        {
            yield return null;
        }

        public virtual void OnGameUpdate(GameManager parentManager)
        {
        }
        public virtual void OnGameFixedUpdate(GameManager parentManager)
        {
        }



    }

}
