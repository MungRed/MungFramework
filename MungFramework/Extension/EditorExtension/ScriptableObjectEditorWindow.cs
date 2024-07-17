#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using UnityEngine;


namespace MungFramework.EditorExtension
{

    /// <summary>
    /// SO编辑器的抽象类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ScriptableObjectEditorWindow<T> : OdinMenuEditorWindow where T : ScriptableObject
    {
        /// <summary>
        /// 标题
        /// </summary>
        public abstract string Title
        {
            get;
        }

        /// <summary>
        /// 配置文件的路径
        /// </summary>
        public abstract string Path
        {
            get;
        }
        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            tree.AddAllAssetsAtPath(Title, Path, typeof(T), true, false);
            return tree;
        }

    }
}
#endif
