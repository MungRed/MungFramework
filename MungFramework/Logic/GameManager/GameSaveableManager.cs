using MungFramework.Logic.Save;

namespace MungFramework.Logic
{
    /// <summary>
    /// 游戏管理器，能调用子管理器和子执行器
    /// </summary>
    public abstract class GameSaveableManager : GameManager
    {
        protected SaveManager @SaveManager;

        public override void OnSceneLoad(GameManager parentManager)
        {
            base.OnSceneLoad(parentManager);

            SaveManager = GameApplication.Instance.SaveManager;
            //把自身添加到存档管理器中
            SaveManager.AddManager(this);
        }


        /// <summary>
        /// 保存到存档
        /// </summary>
        public abstract void Save();
        /// <summary>
        /// 从存档中读取
        /// </summary>
        public abstract void Load();
    }

}
