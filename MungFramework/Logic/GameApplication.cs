using Sirenix.Utilities;
using System.Collections.Generic;
using UnityEngine;
using MungFramework.Input;
using MungFramework.Save;

namespace MungFramework.Logic
{
    /// <summary>
    /// ��ϷӦ�ó�����Ϸ����ڣ�ÿ����������ֻ��һ��GameApplication
    /// </summary>
    public abstract class GameApplication : GameManager
    {
        //��������
        public static GameApplication Instance;

        public InputManager @InputManager;
        public SaveManager @SaveManager;


        /// <summary>
        /// ��Ϸ״̬
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
        /// ����������ʱ����
        /// ��ÿ��Manager���г�ʼ��
        /// </summary>
        public virtual void DOSceneLoad()
        {
            Debug.Log("OnSceneLoad");
            OnSceneLoad(this);
        }

        /// <summary>
        /// ��Ϸ��ʼʱ����
        /// </summary>
        public virtual void DOGameStart()
        {
            Debug.Log("GameStart");
            OnGameStart(this);
        }

        /// <summary>
        /// ÿһ֡�ĵ���
        /// </summary>
        public virtual void DOGameUpdate()
        {
            OnGameUpdate(this);
        }

        /// <summary>
        /// ÿһ�̶�֡�ĵ���
        /// </summary>
        public virtual void DOGameFixedUpdate()
        {
            OnGameFixedUpdate(this);
        }

        /// <summary>
        /// ��Ϸ��ͣ
        /// </summary>
        public virtual void DOGamePause()
        {
            Debug.Log("GamePause");
            OnGamePause(this);
        }

        /// <summary>
        /// ��Ϸ�ָ�
        /// </summary>
        public virtual void DOGameResume()
        {
            Debug.Log("GameResume");
            OnGameResume(this);
        }

    }
}

