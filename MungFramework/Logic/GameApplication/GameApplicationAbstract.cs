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

        #endregion

        /// <summary>
        /// 当场景加载时调用
        /// 对每个Manager进行初始化
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
        /// 游戏开始时调用
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
        /// 游戏暂停
        /// </summary>
        public virtual void DOGamePause()
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
        public virtual void DOGameResume()
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

        /// <summary>
        /// 重新载入场景，用于完成某个任务后刷新npc等
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

