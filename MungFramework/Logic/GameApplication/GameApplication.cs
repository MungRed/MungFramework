using System.Collections;
using UnityEngine;

namespace MungFramework.Logic
{
    /// <summary>
    /// 游戏应用程序，游戏的入口，每个场景有且只有一个GameApplication
    /// </summary>
    public abstract class GameApplication : GameManager
    {
        //单例对象
        public static GameApplication Instance;

        public Input.InputManager InputManager;
        public Save.SaveManager SaveManager;
        public Sound.SoundManager SoundManager;

        

        /// <summary>
        /// 游戏状态
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


        #region Unity消息
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
        /// 当场景加载时调用
        /// 对每个Manager进行初始化
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
        /// 游戏开始时调用
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
        /// 每一帧的调用
        /// </summary>
        public virtual void DOGameUpdate()
        {
            OnGameUpdate(this);
        }

        /// <summary>
        /// 每一固定帧的调用
        /// </summary>
        public virtual void DOGameFixedUpdate()
        {
            OnGameFixedUpdate(this);
        }

        /// <summary>
        /// 游戏暂停
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
            //暂停中
            GameState = GameStateEnum.PauseIn;
            yield return  StartCoroutine(base.OnGamePause(parentManager));
            Debug.Log("GamePause");
            //暂停
            GameState = GameStateEnum.Pause;
        }

        /// <summary>
        /// 游戏恢复
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

