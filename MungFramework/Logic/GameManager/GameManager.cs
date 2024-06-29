using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MungFramework.Logic
{
    /// <summary>
    /// 游戏管理器，能调用子管理器和子执行器
    /// </summary>
    public abstract class GameManager : MonoBehaviour
    {

        /// <summary>
        /// 按一定顺序获取所有的子管理器
        /// </summary>
        /// <returns></returns>
        public List<GameManager> SubGameManagers;

        /// <summary>
        /// 按一定顺序获取所有的子执行器
        /// </summary>
        /// <returns></returns>
        public List<GameExecutor> SubGameExecutors;



        public virtual IEnumerator OnSceneLoad(GameManager parentManager)
        {
            foreach (var subManager in SubGameManagers)
            {
                yield return StartCoroutine(subManager.OnSceneLoad(this));
            }
            foreach (var subExecutors in SubGameExecutors)
            {
                yield return StartCoroutine(subExecutors.OnSceneLoad(this));
            }
            yield return null;
        }
        public virtual IEnumerator OnGameStart(GameManager parentManager)
        {
            foreach (var subManager in SubGameManagers)
            {
                yield return StartCoroutine(subManager.OnGameStart(this));
            }
            foreach (var subExecutors in SubGameExecutors)
            {
                yield return StartCoroutine(subExecutors.OnGameStart(this));
            }
            yield return null;
        }
        public virtual IEnumerator OnGamePause(GameManager parentManager)
        {
            foreach (var subManager in SubGameManagers)
            {
                yield return StartCoroutine(subManager.OnGamePause(this));
            }
            foreach (var subExecutors in SubGameExecutors)
            {
                yield return StartCoroutine(subExecutors.OnGamePause(this));
            }
            yield return null;
        }
        public virtual IEnumerator OnGameResume(GameManager parentManager)
        {
            foreach (var subManager in SubGameManagers)
            {
                yield return StartCoroutine(subManager.OnGameResume(this));
            }
            foreach (var subExecutors in SubGameExecutors)
            {
                yield return StartCoroutine(subExecutors.OnGameResume(this));
            }
            yield return null;
        }

        public virtual void OnGameUpdate(GameManager parentManager)
        {
            SubGameManagers.ForEach(m => m.OnGameUpdate(this));
            SubGameExecutors.ForEach(m => m.OnGameUpdate(this));

        }
        public virtual void OnGameFixedUpdate(GameManager parentManager)
        {
            SubGameManagers.ForEach(m => m.OnGameFixedUpdate(this));
            SubGameExecutors.ForEach(m => m.OnGameFixedUpdate(this));
        }

    }

}
