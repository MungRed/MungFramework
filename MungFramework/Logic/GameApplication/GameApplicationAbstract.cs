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
                OnGameUpdate(this);
            }
        }
        public virtual void FixedUpdate()
        {
            if (GameState == GameStateEnum.Update)
            {
                OnGameFixedUpdate(this);
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
            yield return OnSceneLoadExtra(parentManager);
            GameState = GameStateEnum.Start;
        }
        public virtual IEnumerator OnSceneLoadExtra(GameManagerAbstract parentManager)
        {
            yield return base.OnSceneLoad(parentManager);
        }

        /// <summary>
        /// ��Ϸ��ʼʱ����
        /// </summary>
        public virtual void DOGameStart()
        {
            StartCoroutine(OnGameStart(this));
        }

        public new IEnumerator OnGameStart(GameManagerAbstract parentManager)
        {
            yield return new WaitUntil(()=>GameState == GameStateEnum.Start);
            Debug.Log("GameStart");
            yield return OnGameStartExtra(parentManager);
            GameState = GameStateEnum.Update;
        }
        public virtual IEnumerator OnGameStartExtra(GameManagerAbstract parentManager)
        {
            base.OnGameStart(parentManager);
            yield return null;
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
        }

        public virtual new IEnumerator OnGameReload(GameManagerAbstract parentManager)
        {
            Debug.Log("GameReload");
            GameState = GameStateEnum.Reload;
            yield return OnGameReloadExtra(parentManager);
            yield return OnGameReloadFinish(parentManager);
        }
        public virtual IEnumerator OnGameReloadExtra(GameManagerAbstract parentManager)
        {
            base.OnGameReload(parentManager);
            yield break;
        }

        public virtual new IEnumerator OnGameReloadFinish(GameManagerAbstract parentManager)
        {
            yield return OnGameReloadFinishExtra(parentManager);
            GameState = GameStateEnum.Update;
            Debug.Log("GameReloadFinish");
            yield return null;
        }
        public virtual IEnumerator OnGameReloadFinishExtra(GameManagerAbstract parentManager)
        {
            base.OnGameReloadFinish(parentManager);
            yield break;
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

