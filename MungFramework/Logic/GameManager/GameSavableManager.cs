using MungFramework.Logic.Save;
using System.Collections;

namespace MungFramework.Logic
{
    /// <summary>
    /// 可保存的游戏管理器，能调用子管理器和子执行器
    /// </summary>
    public abstract class GameSavableManager : GameManager
    {
        protected SaveManager SaveManager;

        public override IEnumerator OnSceneLoad(GameManager parentManager)
        {
            yield return StartCoroutine(base.OnSceneLoad(parentManager));
            //把自身添加到存档管理器中
            SaveManager = GameApplication.Instance.SaveManager;
            SaveManager.AddManager(this);
        }


        /// <summary>
        /// 保存到存档
        /// </summary>
        public abstract IEnumerator Save();
        /// <summary>
        /// 从存档中读取
        /// </summary>
        public abstract IEnumerator Load();
    }

}
