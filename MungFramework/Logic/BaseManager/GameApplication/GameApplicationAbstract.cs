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
        public virtual void LateUpdate()
        {
            if (GameState == GameStateEnum.Update)
            {
                OnGameLateUpdate(this);
            }
        }
        #endregion

        #region SceneLoad
        /// <summary>
        /// ����������ʱ����
        /// ��ÿ��Manager���г�ʼ��
        /// </summary>
        public void DOSceneLoad()
        {
            StartCoroutine(OnSceneLoadIEnumerator(this));
        }
        public IEnumerator OnSceneLoadIEnumerator(GameManagerAbstract parentManager)
        {
            Debug.Log("SceneLoad");
            GameState = GameStateEnum.Awake;
            yield return OnSceneLoadIEnumeratorExtra(parentManager);
            GameState = GameStateEnum.Start;
        }
        public virtual IEnumerator OnSceneLoadIEnumeratorExtra(GameManagerAbstract parentManager)
        {
            RegisterEventOnSceneLoad();
            foreach (var subManager in subGameManagerList)
            {
                subManager.OnSceneLoad(this);
                if (subManager is IOnSceneLoadIEnumerator ienumerator)
                {
                    yield return ienumerator.OnSceneLoadIEnumerator(this);
                }
            }
            yield return null;
        }
        #endregion

        #region GameStart
        /// <summary>
        /// ��Ϸ��ʼʱ����
        /// </summary>
        public void DOGameStart()
        {
            StartCoroutine(OnGameStartIEnumerator(this));
        }
        public IEnumerator OnGameStartIEnumerator(GameManagerAbstract parentManager)
        {
            yield return new WaitUntil(() => GameState == GameStateEnum.Start);
            Debug.Log("GameStart");
            yield return OnGameStartIEnumeratorExtra(parentManager);
            GameState = GameStateEnum.Update;
        }
        public virtual IEnumerator OnGameStartIEnumeratorExtra(GameManagerAbstract parentManager)
        {
            base.OnGameStart(parentManager);
            yield return null;
        }
        #endregion

        #region GamePause
        /// <summary>
        /// ��Ϸ��ͣ
        /// </summary>
        public void DOGamePause()
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
        public void DOGameResume()
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
        #endregion

        #region GameReload
        /// <summary>
        /// �������볡�����������ĳ�������ˢ��npc��
        /// </summary>
        public void DOGameReload()
        {
            StartCoroutine(OnGameReloadIEnumerator(this));
        }

        public IEnumerator OnGameReloadIEnumerator(GameManagerAbstract parentManager)
        {
            Debug.Log("GameReload");
            GameState = GameStateEnum.Reload;
            yield return OnGameReloadIEnumeratorExtra(parentManager);
            yield return OnGameReloadIEnumeratorFinish(parentManager);
        }

        public virtual IEnumerator OnGameReloadIEnumeratorExtra(GameManagerAbstract parentManager)
        {
            base.OnGameReload(parentManager);
            yield break;
        }

        public IEnumerator OnGameReloadIEnumeratorFinish(GameManagerAbstract parentManager)
        {
            yield return OnGameReloadFinishIEnumerator(parentManager);
            GameState = GameStateEnum.Update;
            Debug.Log("GameReloadFinish");
            yield return null;
        }

        public virtual IEnumerator OnGameReloadFinishIEnumerator(GameManagerAbstract parentManager)
        {
            base.OnGameReloadFinish(parentManager);
            yield break;
        }
        #endregion

        #region GameQuit
        public void DOGameQuit()
        {
            StartCoroutine(OnGameQuitIEnumerator(this));
        }

        public IEnumerator OnGameQuitIEnumerator(GameManagerAbstract parentManager)
        {
            GameState = GameStateEnum.Quit;
            Debug.Log("GameQuit");
            base.OnGameQuit(parentManager);
            yield return null;
            Application.Quit();
        }
        #endregion
    }
}

