using Sirenix.OdinInspector;
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
        [FoldoutGroup("事件")]
        [LabelText("事件")]
        protected GameManagerEvents GameManagerEvents = new();

        /// <summary>
        /// 按一定顺序获取所有的子管理器
        /// </summary>
        /// <returns></returns>
        [FoldoutGroup("子管理器")]
        [LabelText("子管理器")]
        public List<GameManagerAbstract> SubGameManagerList;

        /// <summary>
        /// 按一定顺序获取所有的子执行器
        /// </summary>
        /// <returns></returns>
        [FoldoutGroup("子管理器")]
        [LabelText("子执行器")]
        public List<GameExecutorAbstract> SubGameExecutorList;



        public virtual IEnumerator OnSceneLoad(GameManagerAbstract parentManager)
        {
            GameManagerEvents.GetEvent(GameManagerEvents.GameMangerEventsEnum.OnSceneLoad)?.Invoke();
            foreach (var subManager in SubGameManagerList)
            {
                yield return subManager.OnSceneLoad(this);
            }
            foreach (var subExecutors in SubGameExecutorList)
            {
                yield return subExecutors.OnSceneLoad(this);
            }
            yield return null;
        }
        public virtual IEnumerator OnGameStart(GameManagerAbstract parentManager)
        {
            GameManagerEvents.GetEvent(GameManagerEvents.GameMangerEventsEnum.OnGameStart)?.Invoke();
            foreach (var subManager in SubGameManagerList)
            {
                yield return subManager.OnGameStart(this);
            }
            foreach (var subExecutors in SubGameExecutorList)
            {
                yield return subExecutors.OnGameStart(this);
            }
            yield return null;
        }
        public virtual IEnumerator OnGamePause(GameManagerAbstract parentManager)
        {
            GameManagerEvents.GetEvent(GameManagerEvents.GameMangerEventsEnum.OnGamePause)?.Invoke();
            foreach (var subManager in SubGameManagerList)
            {
                yield return subManager.OnGamePause(this);
            }
            foreach (var subExecutors in SubGameExecutorList)
            {
                yield return subExecutors.OnGamePause(this);
            }
            yield return null;
        }
        public virtual IEnumerator OnGameResume(GameManagerAbstract parentManager)
        {
            GameManagerEvents.GetEvent(GameManagerEvents.GameMangerEventsEnum.OnGameResume)?.Invoke();
            foreach (var subManager in SubGameManagerList)
            {
                yield return subManager.OnGameResume(this);
            }
            foreach (var subExecutors in SubGameExecutorList)
            {
                yield return subExecutors.OnGameResume(this);
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
