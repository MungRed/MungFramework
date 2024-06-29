using System.Collections;
using UnityEngine;

namespace MungFramework.Logic
{
    /// <summary>
    /// ��ϷӦ�ó�����Ϸ����ڣ�ÿ����������ֻ��һ��GameApplication
    /// </summary>
    public abstract class GameApplication : GameManager
    {
        //��������
        public static GameApplication Instance;

        public Input.InputManager InputManager;
        public Save.SaveManager SaveManager;
        public Sound.SoundManager SoundManager;

        

        /// <summary>
        /// ��Ϸ״̬
        /// </summary>
        public enum GameStateEnum
        {
            Awake,
            Start,
            Update,
            PauseIn,
            Pause,
        }
        public abstract GameStateEnum GameState
        {
            get;
            set;
        }


        #region Unity��Ϣ
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
        #endregion

        /// <summary>
        /// ����������ʱ����
        /// ��ÿ��Manager���г�ʼ��
        /// </summary>
        public virtual void DOSceneLoad()
        {
            StartCoroutine(OnSceneLoad(this));
        }
        public override IEnumerator OnSceneLoad(GameManager parentManager)
        {
            Debug.Log("SceneLoad");
            GameState = GameStateEnum.Awake;
            yield return StartCoroutine(base.OnSceneLoad(parentManager));
            GameState = GameStateEnum.Start;
        }
        /// <summary>
        /// ��Ϸ��ʼʱ����
        /// </summary>
        public virtual void DOGameStart()
        {
            StartCoroutine(OnGameStart(this));
        }
        public override IEnumerator OnGameStart(GameManager parentManager)
        {
            yield return new WaitUntil(()=>GameState == GameStateEnum.Start);


            Debug.Log("GameStart");
            yield return StartCoroutine(base.OnGameStart(parentManager));
            GameState = GameStateEnum.Update;
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
            if (GameState == GameStateEnum.Update)
            {
                StartCoroutine(OnGamePause(this));
            }
        }
        public override IEnumerator OnGamePause(GameManager parentManager)
        {
            //��ͣ��
            GameState = GameStateEnum.PauseIn;
            yield return  StartCoroutine(base.OnGamePause(parentManager));
            Debug.Log("GamePause");
            //��ͣ
            GameState = GameStateEnum.Pause;
        }

        /// <summary>
        /// ��Ϸ�ָ�
        /// </summary>
        public virtual void DOGameResume()
        {
            if (GameState == GameStateEnum.Pause)
            {
                StartCoroutine(OnGameResume(this));
            }
        }
        public override IEnumerator OnGameResume(GameManager parentManager)
        {
            yield return StartCoroutine(base.OnGameResume(parentManager));
            Debug.Log("GameResume");
            GameState = GameStateEnum.Update;
        }


    }
}

