using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace MungFramework.Logic
{
    /// <summary>
    /// ��ϷӦ�ó�����Ϸ����ڣ�ÿ����������ֻ��һ��GameApplication
    /// </summary>
    public abstract class GameApplicationAbstract : GameManagerAbstract
    {
        //��������
        public static GameApplicationAbstract Instance;

        public Save.SaveManagerAbstract SaveManager;
        public Input.InputManagerAbstract InputManager;
        public Sound.SoundManagerAbstract SoundManager;

        

        /// <summary>
        /// ��Ϸ״̬
        /// </summary>
        public enum GameStateEnum
        {
            Awake,
            Start,
            Update,
            PauseIn,//��ͣ��
            Pause,//��ͣ״̬
            ResumeIn,//�ָ���
        }

        [SerializeField]
        [ReadOnly]
        protected GameStateEnum GameState;


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
        public override IEnumerator OnSceneLoad(GameManagerAbstract parentManager)
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
        public override IEnumerator OnGameStart(GameManagerAbstract parentManager)
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
            //ֻ������Ϸ����״̬�²�����ͣ
            if (GameState == GameStateEnum.Update)
            {
                StartCoroutine(OnGamePause(this));
            }
        }
        public override IEnumerator OnGamePause(GameManagerAbstract parentManager)
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
            //ֻ������Ϸ��ͣ״̬�²��ָܻ�
            if (GameState == GameStateEnum.Pause)
            {
                StartCoroutine(OnGameResume(this));
            }
        }
        public override IEnumerator OnGameResume(GameManagerAbstract parentManager)
        {
            //�ָ���
            GameState = GameStateEnum.ResumeIn;
            yield return StartCoroutine(base.OnGameResume(parentManager));
            Debug.Log("GameResume");
            GameState = GameStateEnum.Update;
        }


    }
}

