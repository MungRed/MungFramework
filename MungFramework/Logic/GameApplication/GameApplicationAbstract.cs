using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
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

            Reload,//重新载入
            Quit,//退出

            PauseIn,//暂停中
            Pause,//暂停状态
            ResumeIn,//恢复中
        }

        [SerializeField]
        [ReadOnly]
        public GameStateEnum GameState;


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
        public override IEnumerator OnSceneLoad(GameManagerAbstract parentManager)
        {
            Debug.Log("SceneLoad");
            GameState = GameStateEnum.Awake;
            yield return base.OnSceneLoad(parentManager);
            GameState = GameStateEnum.Start;
        }
        /// <summary>
        /// 游戏开始时调用
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
            //只有在游戏更新状态下才能暂停
            if (GameState == GameStateEnum.Update)
            {
                StartCoroutine(OnGamePause(this));
            }
        }
        public override IEnumerator OnGamePause(GameManagerAbstract parentManager)
        {
            //暂停中
            GameState = GameStateEnum.PauseIn;
            yield return  base.OnGamePause(parentManager);
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
                StartCoroutine(OnGameResume(this));
            }
        }
        public override IEnumerator OnGameResume(GameManagerAbstract parentManager)
        {
            //恢复中
            GameState = GameStateEnum.ResumeIn;
            yield return base.OnGameResume(parentManager);
            Debug.Log("GameResume");
            GameState = GameStateEnum.Update;
        }

        /// <summary>
        /// 重新载入场景，用于完成某个任务后刷新npc等
        /// </summary>
        public virtual void DOGameReload()
        {
            //只有在游戏更新状态下才能重新载入
            if (GameState == GameStateEnum.Update)
            {
                StartCoroutine(OnGameReload(this));
            }
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

