using Sirenix.Utilities;
using System.Collections.Generic;
using UnityEngine;
using MungFramework.Input;
using MungFramework.Save;

namespace MungFramework.Logic
{
    /// <summary>
    /// 游戏应用程序，游戏的入口，每个场景有且只有一个GameApplication
    /// </summary>
    public abstract class GameApplication : GameManager
    {
        //单例对象
        public static GameApplication Instance;

        public InputManager @InputManager;
        public SaveManager @SaveManager;


        /// <summary>
        /// 游戏状态
        /// </summary>
        public enum GameStateEnum
        {
            Start,
            Update,
            Pause,
        }
        public abstract GameStateEnum GameState
        {
            get;
            set;
        }


        public virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            DOSceneLoad();
        }

        public virtual void Start()
        {
            DOGameStart();
        }

        public virtual void Update()
        {
            if (GameState == GameStateEnum.Update)
            {
                DOGameUpdate();
            }
        }

        public virtual void FixedUpdate()
        {
            if (GameState == GameStateEnum.Update)
            {
                DOGameFixedUpdate();
            }
        }


        /// <summary>
        /// 当场景加载时调用
        /// 对每个Manager进行初始化
        /// </summary>
        public virtual void DOSceneLoad()
        {
            Debug.Log("OnSceneLoad");
            OnSceneLoad(this);
        }

        /// <summary>
        /// 游戏开始时调用
        /// </summary>
        public virtual void DOGameStart()
        {
            Debug.Log("GameStart");
            OnGameStart(this);
        }

        /// <summary>
        /// 每一帧的调用
        /// </summary>
        public virtual void DOGameUpdate()
        {
            OnGameUpdate(this);
        }

        /// <summary>
        /// 每一固定帧的调用
        /// </summary>
        public virtual void DOGameFixedUpdate()
        {
            OnGameFixedUpdate(this);
        }

        /// <summary>
        /// 游戏暂停
        /// </summary>
        public virtual void DOGamePause()
        {
            Debug.Log("GamePause");
            OnGamePause(this);
        }

        /// <summary>
        /// 游戏恢复
        /// </summary>
        public virtual void DOGameResume()
        {
            Debug.Log("GameResume");
            OnGameResume(this);
        }

    }
}

