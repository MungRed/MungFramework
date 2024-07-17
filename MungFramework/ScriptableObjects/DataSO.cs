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
        private void SetId()
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = name;
                UnityEditor.EditorUtility.SetDirty(this);
            }
        }
#endif
    }
}
