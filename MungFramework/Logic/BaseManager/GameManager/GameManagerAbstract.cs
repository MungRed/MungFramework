﻿using MungFramework.Extension.ComponentExtension;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MungFramework.Logic
{
    /// <summary>
    /// 游戏管理器，能调用子管理器和子控制器
    /// </summary>
    public abstract class GameManagerAbstract : MonoBehaviour
    {
        /// <summary>
        /// 按一定顺序获取所有的子管理器
        /// </summary>
        /// <returns></returns>
        [LabelText("子管理器")]
        [SerializeField]
        protected List<GameManagerAbstract> subGameManagerList;

        /// <summary>
        /// 按一定顺序获取所有的子执行器
        /// </summary>
        /// <returns></returns>
        [LabelText("子控制器")]
        [SerializeField]
        protected List<GameControllerAbstract> subGameControllerList;

        public virtual IEnumerator OnSceneLoad(GameManagerAbstract parentManager)
        {
            RegisterEventOnSceneLoad();
            foreach (var subManager in subGameManagerList)
            {
                yield return subManager.OnSceneLoad(this);
            }
            foreach (var subController in subGameControllerList)
            {
                subController.OnSceneLoad(this);
            }
        }

        protected virtual void RegisterEventOnSceneLoad()
        {

        }

        public virtual void OnGameStart(GameManagerAbstract parentManager)
        {
            foreach (var subManager in subGameManagerList)
            {
                subManager.OnGameStart(this);
            }
            foreach (var subController in subGameControllerList)
            {
                subController.OnGameStart(this);
            }
            StartCoroutine(OnGameStartIEnumerator(parentManager));
        }

        public virtual IEnumerator OnGameStartIEnumerator(GameManagerAbstract parentManager)
        {
            yield return null;
        }

        public virtual void OnGamePause(GameManagerAbstract parentManager)
        {
            foreach (var subManager in subGameManagerList)
            {
                subManager.OnGamePause(this);
            }
            foreach (var subController in subGameControllerList)
            {
                subController.OnGamePause(this);
            }
        }

        public virtual void OnGameResume(GameManagerAbstract parentManager)
        {
            foreach (var subManager in subGameManagerList)
            {
                subManager.OnGameResume(this);
            }
            foreach (var subController in subGameControllerList)
            {
                subController.OnGameResume(this);
            }
        }

        public virtual void OnGameReload(GameManagerAbstract parentManager)
        {
            foreach (var subManager in subGameManagerList)
            {
                subManager.OnGameReload(this);
            }
            foreach (var subController in subGameControllerList)
            {
                subController.OnGameReload(this);
            }
        }

        public virtual void OnGameReloadFinish(GameManagerAbstract parentManager)
        {
            foreach (var subManager in subGameManagerList)
            {
                subManager.OnGameReloadFinish(this);
            }
            foreach (var subController in subGameControllerList)
            {
                subController.OnGameReloadFinish(this);
            }
        }

        public virtual IEnumerator OnGameQuit(GameManagerAbstract parentManager)
        {
            foreach (var subManager in subGameManagerList)
            {
                yield return subManager.OnGameQuit(this);
            }
            foreach (var subController in subGameControllerList)
            {
                subController.OnGameQuit(this);
            }
            yield return null;
        }

        public virtual void OnGameUpdate(GameManagerAbstract parentManager)
        {
            foreach (var subManager in subGameManagerList)
            {
                subManager.OnGameUpdate(this);
            }
            foreach (var subController in subGameControllerList)
            {
                subController.OnGameUpdate(this);
            }
        }
        public virtual void OnGameFixedUpdate(GameManagerAbstract parentManager)
        {
            foreach (var subManager in subGameManagerList)
            {
                subManager.OnGameFixedUpdate(this);
            }
            foreach (var subController in subGameControllerList)
            {
                subController.OnGameFixedUpdate(this);
            }
        }
        public virtual void OnGameLateUpdate(GameManagerAbstract parentManager)
        {
            foreach (var subManager in subGameManagerList)
            {
                subManager.OnGameLateUpdate(this);
            }
            foreach (var subController in subGameControllerList)
            {
                subController.OnGameLateUpdate(this);
            }
        }
#if UNITY_EDITOR
        [InfoBox("$subManagerInfo")]
        [ShowInInspector]
        [PropertyOrder(10)]
        private string subManagerInfo
        {
            get
            {
                string info = "";
                if (subGameControllerList != null && !subGameManagerList.Empty())
                {
                    info += "子管理器：\n";
                    foreach (var subManager in subGameManagerList)
                    {
                        if (subManager != null)
                        {
                            info += subManager.name + "\n";
                        }
                        else
                        {
                            Debug.LogError(name + "子管理器错误，请查看");
                        }

                    }
                }
                if (subGameControllerList != null && !subGameControllerList.Empty())
                {
                    info += "子执行器：\n";
                    foreach (var subExecutor in subGameControllerList)
                    {
                        if (subExecutor != null)
                        {
                            info += subExecutor.name + "\n";
                        }
                        else
                        {
                            Debug.LogError(name + "子执行器错误，请查看");
                        }
                    }
                }
                return info;
            }
        }
#endif
    }

}
