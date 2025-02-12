using MungFramework.Extension.ListExtension;
using Sirenix.OdinInspector;
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
        /// 在场景加载时注册事件
        /// </summary>
        protected virtual void RegisterEventOnSceneLoad()
        {

        }

        public virtual void OnSceneLoad(GameManagerAbstract parentManager)
        {
            RegisterEventOnSceneLoad();
            foreach (var subManager in subGameManagerList)
            {
                subManager.OnSceneLoad(this);
            }
        }

        public virtual void OnGameStart(GameManagerAbstract parentManager)
        {
            foreach (var subManager in subGameManagerList)
            {
                subManager.OnGameStart(this);
            }
        }

        public virtual void OnGamePause(GameManagerAbstract parentManager)
        {
            foreach (var subManager in subGameManagerList)
            {
                subManager.OnGamePause(this);
            }
        }

        public virtual void OnGameResume(GameManagerAbstract parentManager)
        {
            foreach (var subManager in subGameManagerList)
            {
                subManager.OnGameResume(this);
            }
        }

        public virtual void OnGameReload(GameManagerAbstract parentManager)
        {
            foreach (var subManager in subGameManagerList)
            {
                subManager.OnGameReload(this);
            }
        }

        public virtual void OnGameReloadFinish(GameManagerAbstract parentManager)
        {
            foreach (var subManager in subGameManagerList)
            {
                subManager.OnGameReloadFinish(this);
            }
        }

        public virtual void OnGameQuit(GameManagerAbstract parentManager)
        {
            foreach (var subManager in subGameManagerList)
            {
                subManager.OnGameQuit(this);
            }
        }

        public virtual void OnGameUpdate(GameManagerAbstract parentManager)
        {
            foreach (var subManager in subGameManagerList)
            {
                subManager.OnGameUpdate(this);
            }
        }
        public virtual void OnGameFixedUpdate(GameManagerAbstract parentManager)
        {
            foreach (var subManager in subGameManagerList)
            {
                subManager.OnGameFixedUpdate(this);
            }
        }
        public virtual void OnGameLateUpdate(GameManagerAbstract parentManager)
        {
            foreach (var subManager in subGameManagerList)
            {
                subManager.OnGameLateUpdate(this);
            }
        }

#if UNITY_EDITOR
        [Button("挂载所有子管理器")]
        public void FindSubManager()
        {
            subGameManagerList.Clear();
            foreach (var subManager in GetComponentsInChildren<GameManagerAbstract>(true))
            {
                if (subManager != this && subManager.GetComponentsInParent<GameManagerAbstract>(true)[1] == this)
                {
                    subGameManagerList.Add(subManager);
                }
            }
        }

        [InfoBox("$subManagerInfo")]
        [ShowInInspector]
        [PropertyOrder(10)]
        public string subManagerInfo
        {
            get
            {
                string info = "";
                if (subGameManagerList != null && !subGameManagerList.Empty())
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
                return info;
            }
        }
#endif
    }

}
