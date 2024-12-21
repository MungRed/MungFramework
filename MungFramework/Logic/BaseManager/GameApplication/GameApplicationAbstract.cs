using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace MungFramework.Logic
{
    /// <summary>
    /// 游戏应用程序，游戏的入口，每个场景有且只有一个GameApplication
    /// </summary>
    public abstract class GameApplicationAbstract : SingletonGameManagerAbstract<GameApplicationAbstract>
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
            base.OnGameStart(parentManager);
            yield return null;
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
                OnGamePause(this);
            }
        }

        public override void OnGamePause(GameManagerAbstract parentManager)
        {
            base.OnGamePause(parentManager);
            Debug.Log("GamePause");
            //暂停
            GameState = GameStateEnum.Pause;
        }

        /// <summary>
        /// 游戏恢复
        /// </summary>
        public void DOGameResume()
        {
            //只有在游戏暂停状态下才能恢复暂停
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

