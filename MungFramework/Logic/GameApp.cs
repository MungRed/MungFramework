using System.Collections.Generic;
using UnityEngine;

namespace MungFramework.Logic
{
    /// <summary>
    /// ��ϷӦ�ó�����Ϸ����ڣ�ÿ����������ֻ��һ��GameApp
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
        /// ��һ��˳���ȡ���е�Manager
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
        /// ����������ʱ����
        /// ��ÿ��Manager���г�ʼ��
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
        /// ��Ϸ��ʼʱ����
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
        /// ÿһ֡�ĵ���
        /// </summary>
        public virtual void GameUpdate()
        {
            foreach (var manager in GetManagers())
            {
                manager.OnGameUpdate();
            }
        }

        /// <summary>
        /// ÿһ�̶�֡�ĵ���
        /// </summary>
        public virtual void GameFixedUpdate()
        {
            foreach (var manager in GetManagers())
            {
                manager.OnGameFixedUpdate();
            }
        }

        /// <summary>
        /// ��Ϸ��ͣ
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
        /// ��Ϸ�ָ�
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

