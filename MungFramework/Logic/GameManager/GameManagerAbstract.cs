﻿using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MungFramework.Logic
{
    /// <summary>
    /// 游戏管理器，能调用子管理器和子执行器
    /// </summary>
    public abstract class GameManagerAbstract : MonoBehaviour
    {
        [SerializeField]
        protected GameManagerEvents GameManagerEvents = new();

        /// <summary>
        /// 按一定顺序获取所有的子管理器
        /// </summary>
        /// <returns></returns>
        public List<GameManagerAbstract> SubGameManagerList;

        /// <summary>
        /// 按一定顺序获取所有的子执行器
        /// </summary>
        /// <returns></returns>
        public List<GameExecutorAbstract> SubGameExecutorList;



        public virtual IEnumerator OnSceneLoad(GameManagerAbstract parentManager)
        {
            GameManagerEvents.GetEvent(GameManagerEvents.GameMangerEventsEnum.OnSceneLoad)?.Invoke();
            foreach (var subManager in SubGameManagerList)
            {
                yield return StartCoroutine(subManager.OnSceneLoad(this));
            }
            foreach (var subExecutors in SubGameExecutorList)
            {
                yield return StartCoroutine(subExecutors.OnSceneLoad(this));
            }
            yield return null;
        }
        public virtual IEnumerator OnGameStart(GameManagerAbstract parentManager)
        {
            GameManagerEvents.GetEvent(GameManagerEvents.GameMangerEventsEnum.OnGameStart)?.Invoke();
            foreach (var subManager in SubGameManagerList)
            {
                yield return StartCoroutine(subManager.OnGameStart(this));
            }
            foreach (var subExecutors in SubGameExecutorList)
            {
                yield return StartCoroutine(subExecutors.OnGameStart(this));
            }
            yield return null;
        }
        public virtual IEnumerator OnGamePause(GameManagerAbstract parentManager)
        {
            GameManagerEvents.GetEvent(GameManagerEvents.GameMangerEventsEnum.OnGamePause)?.Invoke();
            foreach (var subManager in SubGameManagerList)
            {
                yield return StartCoroutine(subManager.OnGamePause(this));
            }
            foreach (var subExecutors in SubGameExecutorList)
            {
                yield return StartCoroutine(subExecutors.OnGamePause(this));
            }
            yield return null;
        }
        public virtual IEnumerator OnGameResume(GameManagerAbstract parentManager)
        {
            GameManagerEvents.GetEvent(GameManagerEvents.GameMangerEventsEnum.OnGameResume)?.Invoke();
            foreach (var subManager in SubGameManagerList)
            {
                yield return StartCoroutine(subManager.OnGameResume(this));
            }
            foreach (var subExecutors in SubGameExecutorList)
            {
                yield return StartCoroutine(subExecutors.OnGameResume(this));
            }
            yield return null;
        }

        public virtual void OnGameUpdate(GameManagerAbstract parentManager)
        {
            GameManagerEvents.GetEvent(GameManagerEvents.GameMangerEventsEnum.OnGameUpdate)?.Invoke();
            SubGameManagerList.ForEach(m => m.OnGameUpdate(this));
            SubGameExecutorList.ForEach(m => m.OnGameUpdate(this));
        }
        public virtual void OnGameFixedUpdate(GameManagerAbstract parentManager)
        {
            GameManagerEvents.GetEvent(GameManagerEvents.GameMangerEventsEnum.OnGameFixedUpdate)?.Invoke();
            SubGameManagerList.ForEach(m => m.OnGameFixedUpdate(this));
            SubGameExecutorList.ForEach(m => m.OnGameFixedUpdate(this));
        }

    }

}
