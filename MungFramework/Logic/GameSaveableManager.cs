using Sirenix.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace MungFramework.Logic
{
    /// <summary>
    /// 游戏管理器，能调用子管理器和子执行器
    /// </summary>
    public abstract class GameSaveableManager : GameManager
    {
        public override void OnSceneLoad(GameManager parentManager)
        {
            base.OnSceneLoad(parentManager);

            //把自身添加到存档管理器中
            GameApplication.Instance.SaveManager.AddManager(this);
        }


        /// <summary>
        /// 保存到存档
        /// </summary>
        public virtual void Save()
        {
        }
        /// <summary>
        /// 从存档中读取
        /// </summary>
        public virtual void Load()
        {
        }
    }

}
