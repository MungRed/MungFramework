using MungFramework.Logic.Save;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MungFramework.Logic
{
    /// <summary>
    /// 可保存的游戏管理器，只能用于根节点，在开始游戏时读取数据，存档时保存数据
    /// </summary>
    public abstract class SavableGameManagerAbstract : GameManagerAbstract
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

#if UNITY_EDITOR
        [SerializeField]
        private bool notLoadOnStart;
#endif

        public override void OnSceneLoad(GameManagerAbstract parentManager)
        {
            base.OnSceneLoad(parentManager);
            //把自身添加到存档管理器中
            saveManager.AddManager(this);

#if UNITY_EDITOR
            //调试代码，在开始时不加载数据，用于测试
            if (notLoadOnStart == false)
            {
                //加载数据
                Load();
            }
#else
            //加载数据
            Load();
#endif
        }


        /// <summary>
        /// 保存到存档
        /// </summary>
        [Button]
        public abstract void Save();
        /// <summary>
        /// 从存档中读取
        /// </summary>
        [Button]
        public abstract void Load();
    }

}
