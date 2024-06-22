using System.Collections.Generic;
using UnityEngine;

namespace MungFramework.Logic
{
    /// <summary>
    /// 游戏应用程序，游戏的入口，每个场景有且只有一个GameApp
    /// </summary>
    public abstract class GameApp : MonoBehaviour
    {
        public enum GameStateEnum
        {
            Start,
            Update,
            Pause,
            Resume
        }
        public abstract GameStateEnum GameState
        {
            get;
            set;
        }

        /// <summary>
        /// 按一定顺序获取所有的Manager
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<GameManager> GetManagers();



        public virtual void Awake()
        {
            OnSceneLoad();
        }
        public virtual void Start()
        {
            GameStart();
        }
        public virtual void Update()
        {
            if (GameState == GameStateEnum.Update)
            {
                GameUpdate();
            }
        }
        public virtual void FixedUpdate()
        {
            if (GameState == GameStateEnum.Update)
            {
                GameFixedUpdate();
            }
        }


        /// <summary>
        /// 当场景加载时调用
        /// 对每个Manager进行初始化
        /// </summary>
        public virtual void OnSceneLoad()
        {
            Debug.Log("OnSceneLoad");
            foreach (var manager in GetManagers())
            {
                manager.OnSceneLoad();
                Debug.Log(manager.name + "OnSceneLoad");
            }
        }

        /// <summary>
        /// 游戏开始时调用
        /// </summary>
        public virtual void GameStart()
        {
            Debug.Log("GameStart");
            foreach (var manager in GetManagers())
            {
                manager.OnGameStart();
                Debug.Log(manager.name + "GameStart");
            }
        }

        /// <summary>
        /// 每一帧的调用
        /// </summary>
        public virtual void GameUpdate()
        {
            foreach (var manager in GetManagers())
            {
                manager.OnGameUpdate();
            }
        }

        /// <summary>
        /// 每一固定帧的调用
        /// </summary>
        public virtual void GameFixedUpdate()
        {
            foreach (var manager in GetManagers())
            {
                manager.OnGameFixedUpdate();
            }
        }

        /// <summary>
        /// 游戏暂停
        /// </summary>
        public virtual void GamePause()
        {
            Debug.Log("GamePause");
            foreach (var manager in GetManagers())
            {
                manager.OnGamePause();
                Debug.Log(manager.name + "GamePause");
            }
        }

        /// <summary>
        /// 游戏恢复
        /// </summary>
        public virtual void GameResume()
        {
            Debug.Log("GameResume");
            foreach (var manager in GetManagers())
            {
                manager.OnGameResume();
                Debug.Log(manager.name + "GameResume");
            }
        }


    }
}

