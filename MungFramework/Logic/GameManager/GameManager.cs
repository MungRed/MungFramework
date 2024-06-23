using System.Collections.Generic;
using UnityEngine;

namespace MungFramework.Logic
{
    /// <summary>
    /// 游戏管理器，能调用子管理器和子执行器
    /// </summary>
    public abstract class GameManager : MonoBehaviour
    {

        /// <summary>
        /// 按一定顺序获取所有的子管理器
        /// </summary>
        /// <returns></returns>
        public List<GameManager> GameManagers;

        /// <summary>
        /// 按一定顺序获取所有的子执行器
        /// </summary>
        /// <returns></returns>
        public List<GameExecutor> GameExecutors;


        public virtual void OnSceneLoad(GameManager parentManager)
        {
            GameManagers.ForEach(m => m.OnSceneLoad(this));
            GameExecutors.ForEach(m => m.OnSceneLoad(this));
        }
        public virtual void OnGameStart(GameManager parentManage)
        {
            GameManagers.ForEach(m => m.OnGameStart(this));
            GameExecutors.ForEach(m => m.OnGameStart(this));
        }
        public virtual void OnGameUpdate(GameManager parentManage)
        {
            GameManagers.ForEach(m => m.OnGameUpdate(this));
            GameExecutors.ForEach(m => m.OnGameUpdate(this));

        }
        public virtual void OnGameFixedUpdate(GameManager parentManage)
        {
            GameManagers.ForEach(m => m.OnGameFixedUpdate(this));
            GameExecutors.ForEach(m => m.OnGameFixedUpdate(this));
        }
        public virtual void OnGamePause(GameManager parentManage)
        {
            GameManagers.ForEach(m => m.OnGamePause(this));
            GameExecutors.ForEach(m => m.OnGamePause(this));
        }
        public virtual void OnGameResume(GameManager parentManage)
        {
            GameManagers.ForEach(m => m.OnGameResume(this));
            GameExecutors.ForEach(m => m.OnGameResume(this));
        }
    }

}
