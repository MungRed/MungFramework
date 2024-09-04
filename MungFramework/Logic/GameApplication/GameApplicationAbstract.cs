using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace MungFramework.Logic
{
    /// <summary>
    /// ��ϷӦ�ó�����Ϸ����ڣ�ÿ����������ֻ��һ��GameApplication
    /// </summary>
    public abstract class GameApplicationAbstract : SingletonGameManagerAbstract<GameApplicationAbstract>
    {
        /// <summary>
        /// ��Ϸ״̬
        /// </summary>
        public enum GameStateEnum
        {
            Awake,
            Start,
            Update,
            Pause,//��ͣ״̬
            Reload,//��������
            Quit,//�˳�
        }

        [SerializeField]
        [ReadOnly]
        private GameStateEnum gameState;

        public GameStateEnum GameState
        {
            get => gameState;
            protected set => gameState = value;
        }


        #region Unity��Ϣ
        public virtual void Awake()
        {
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
            yield return base.OnSceneLoad(parentManager);
            GameState = GameStateEnum.Start;
        }
        /// <summary>
        /// ��Ϸ��ʼʱ����
        /// </summary>
        public virtual void DOGameStart()
        {
            StartCoroutine(OnGameStart(this));
        }
        public  override IEnumerator OnGameStart(GameManagerAbstract parentManager)
        {
            yield return new WaitUntil(()=>GameState == GameStateEnum.Start);

            Debug.Log("GameStart");
            yield return base.OnGameStart(parentManager);
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
                OnGamePause(this);
            }
        }
        public override void OnGamePause(GameManagerAbstract parentManager)
        {
            base.OnGamePause(parentManager);
            Debug.Log("GamePause");
            //��ͣ
            GameState = GameStateEnum.Pause;
        }

        /// <summary>
        /// ��Ϸ�ָ�
        /// </summary>
        public virtual void DOGameResume()
        {
            //ֻ������Ϸ��ͣ״̬�²��ָܻ���ͣ
            if (GameState == GameStateEnum.Pause)
            {
                OnGameResume(this);
            }
        }
        public override void OnGameResume(GameManagerAbstract parentManager)
        {
            base.OnGameResume(parentManager);
            Debug.Log("GameResume");
            GameState = GameStateEnum.Update;
        }

        /// <summary>
        /// �������볡�����������ĳ�������ˢ��npc��
        /// </summary>
        public virtual void DOGameReload()
        {
            StartCoroutine(OnGameReload(this));
            //ֻ������Ϸ����״̬�²�����������
            //if (GameState == GameStateEnum.Update)
            //{
            //    StartCoroutine(OnGameReload(this));
            //}
        }
        public virtual new IEnumerator OnGameReload(GameManagerAbstract parentManager)
        {
            Debug.Log("GameReload");
            GameState = GameStateEnum.Reload;
            base.OnGameReload(parentManager);
            yield return OnGameReloadFinish(parentManager);
        }
        public virtual new IEnumerator OnGameReloadFinish(GameManagerAbstract parentManager)
        {
            base.OnGameReloadFinish(parentManager);
            GameState = GameStateEnum.Update;
            Debug.Log("GameReloadFinish");
            yield return null;
        }
        public virtual void DOGameQuit()
        {
            StartCoroutine(OnGameQuit(this));
        }
        public override IEnumerator OnGameQuit(GameManagerAbstract parentManager)
        {
            GameState = GameStateEnum.Quit;
            Debug.Log("GameQuit");
            yield return base.OnGameQuit(parentManager);
            Application.Quit();
        }


    }
}

