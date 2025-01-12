using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace MungFramework.Logic
{
    /// <summary>
    /// 游戏应用程序，游戏的入口，每个场景有且只有一个GameApplication
    /// </summary>
    public abstract class GameApplicationAbstract : SingletonGameManagerAbstract<GameApplicationAbstract>, IOnSceneLoadEnumerator, IOnGameStartEnumerator, IOnGameReloadEnumerator, IOnGameReloadFinishEnumerator,IOnGamePauseEnumerator,IOnGameResumeEnumerator,IOnGameQuitEnumerator
    {
        /// <summary>
        /// 游戏状态
        /// </summary>
        public enum GameStateEnum
        {
            Awake,
            Start,
            Update,
            Pause,//暂停状态
            Reload,//重新载入
            Quit,//退出
        }

        [SerializeField]
        [ReadOnly]
        private GameStateEnum gameState;
        public GameStateEnum GameState
        {
            get => gameState;
            protected set => gameState = value;
        }

        #region Unity消息
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
        /// 当场景加载时调用
        /// 对每个Manager进行初始化
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
                if (subManager is IOnSceneLoadEnumerator ienumerator)
                {
                    yield return ienumerator.OnSceneLoadIEnumerator(this);
                }
            }
            yield break;
        }
        #endregion

        #region GameStart
        /// <summary>
        /// 游戏开始时调用
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
            foreach (var subManager in subGameManagerList)
            {
                subManager.OnGameStart(this);
                if (subManager is IOnGameStartEnumerator ienumerator)
                {
                    yield return ienumerator.OnGameStartIEnumerator(this);
                }
            }
            yield break;
        }
        #endregion

        #region GamePause
        /// <summary>
        /// 游戏暂停
        /// </summary>
        public void DOGamePause()
        {
            //只有在游戏更新状态下才能暂停
            if (GameState == GameStateEnum.Update)
            {
                StartCoroutine(OnGamePauseIEnumerator(this));
            }
        }
        public IEnumerator OnGamePauseIEnumerator(GameManagerAbstract parentManager)
        {
            Debug.Log("GamePause");
            GameState = GameStateEnum.Pause;
            foreach (var subManager in subGameManagerList)
            {
                subManager.OnGamePause(this);
                if (subManager is IOnGamePauseEnumerator ienumerator)
                {
                    yield return ienumerator.OnGamePauseIEnumerator(this);
                }
            }
            yield break;
        }

        /// <summary>
        /// 游戏恢复
        /// </summary>
        public void DOGameResume()
        {
            //只有在游戏暂停状态下才能恢复暂停
            if (GameState == GameStateEnum.Pause)
            {
                StartCoroutine(OnGameResumeIEnumerator(this));
            }
        }
        public IEnumerator OnGameResumeIEnumerator(GameManagerAbstract parentManager)
        {
            Debug.Log("GameResume");
            foreach (var subManager in subGameManagerList)
            {
                subManager.OnGameResume(this);
                if (subManager is IOnGameResumeEnumerator ienumerator)
                {
                    yield return ienumerator.OnGameResumeIEnumerator(this);
                }
            }
            GameState = GameStateEnum.Update;
            yield break;
        }
        #endregion

        #region GameReload
        /// <summary>
        /// 重新载入场景，用于完成某个任务后刷新npc等
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

            yield return OnGameReloadFinishIEnumerator(parentManager);
            GameState = GameStateEnum.Update;
            Debug.Log("GameReloadFinish");
        }

        public virtual IEnumerator OnGameReloadIEnumeratorExtra(GameManagerAbstract parentManager)
        {
            foreach (var subManager in subGameManagerList)
            {
                subManager.OnGameReload(this);
                if (subManager is IOnGameReloadEnumerator ienumerator)
                {
                    yield return ienumerator.OnGameReloadIEnumerator(this);
                }
            }
            yield break;
        }

        public IEnumerator OnGameReloadFinishIEnumerator(GameManagerAbstract parentManager)
        {
            yield return OnGameReloadFinishIEnumeratorExtra(parentManager);
        }
        public virtual IEnumerator OnGameReloadFinishIEnumeratorExtra(GameManagerAbstract parentManager)
        {
            foreach (var subManager in subGameManagerList)
            {
                subManager.OnGameReloadFinish(this);
                if (subManager is IOnGameReloadFinishEnumerator ienumerator)
                {
                    yield return ienumerator.OnGameReloadFinishIEnumerator(this);
                }
            }
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
            foreach (var subManager in subGameManagerList)
            {
                subManager.OnGameQuit(this);
                if (subManager is IOnGameQuitEnumerator ienumerator)
                {
                    yield return ienumerator.OnGameQuitIEnumerator(this);
                }
            }
            yield return null;
            Application.Quit();
        }
        #endregion
    }
}

