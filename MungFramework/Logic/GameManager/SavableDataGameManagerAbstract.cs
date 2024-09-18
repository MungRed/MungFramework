using MungFramework.Logic.Save;
using System.Collections;

namespace MungFramework.Logic
{
    /// <summary>
    /// 可保存的游戏管理器，只能用于根节点，在开始游戏时读取数据，存档时保存数据
    /// </summary>
    public abstract class SavableDataGameManagerAbstract : DataGameManagerAbstract
    {
        private SaveManagerAbstract _saveManager;
        protected SaveManagerAbstract saveManager
        {
            get
            {
                if (_saveManager == null)
                {
                    _saveManager = SaveManagerAbstract.Instance;
                }
                return _saveManager;
            }
        }

        public override void  OnSceneLoad(GameManagerAbstract parentManager)
        {
            base.OnSceneLoad(parentManager);
            //把自身添加到存档管理器中
            saveManager.AddManager(this);
            //加载数据
            Load();
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
