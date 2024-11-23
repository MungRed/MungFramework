using Sirenix.OdinInspector;
using UnityEngine;
namespace MungFramework.ScriptableObjects
{
    /// <summary>
    /// 封装的ScriptableObject基类
    /// </summary>
    public abstract class DataSO : ScriptableObject
    {
        public string Id;


#if UNITY_EDITOR
        //将id设置为资源名称
        [Button("将id设置为资源名称", ButtonSizes.Medium)]
        public virtual void SetId()
        {
            Id = name;
            UnityEditor.EditorUtility.SetDirty(this);
        }
        //将资源名称设置为id
        [Button("将资源名称设置为id", ButtonSizes.Medium)]
        public virtual void SetAssetName()
        {
            UnityEditor.AssetDatabase.RenameAsset(UnityEditor.AssetDatabase.GetAssetPath(this), Id);
        }
#endif
    }
}
