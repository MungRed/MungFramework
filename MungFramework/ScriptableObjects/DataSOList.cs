using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

namespace MungFramework.ScriptableObjects
{
    /// <summary>
    /// ScriptableObject列表基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DataSOList<T> : ScriptableObject where T : DataSO
    {
        [Serializable]
        public class DataSOItem
        {
            public string id;
            public T item;

            public DataSOItem(string id, T item)
            {
                this.id = id;
                this.item = item;
            }
        }

        private string TTypeName => typeof(T).Name;

        [SerializeField]
        protected List<DataSOItem> TList = new();

        [Button("@\"获取所有\" + TTypeName + \"文件\"",ButtonSizes.Medium)]
        private void GetAllSO()
        {
            TList.Clear();
            var guids = UnityEditor.AssetDatabase.FindAssets("t:" + typeof(T).Name);
            foreach (var guid in guids)
            {
                var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var so = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
                TList.Add(new(so.id,so));
            }
            SortList();
        }

        [Button("对id排序", ButtonSizes.Medium)]
        private void SortList()
        {
            TList.Sort((a, b) => string.Compare(a.id, b.id, StringComparison.Ordinal));
            Debug.Log("已排序");
        }

        [Button("检查id重复性和正确性", ButtonSizes.Medium)]
        private void CheckID()
        {
            bool flag = true;
            HashSet<string> idSet = new HashSet<string>();
            foreach (var t in TList)
            {
                if (t.id.Contains(" "))
                {
                    Debug.LogError($"id包含空格(已经清除)：id:{t.id} name:{t.item.name}");
                    t.id = t.id.Replace(" ", "");
                }
                if (idSet.Contains(t.id))
                {
                    flag = false;
                    Debug.LogError($"id重复：id:{t.id} name:{t.item.name}");
                }
                idSet.Add(t.id);
            }

            if (flag)
            {
                Debug.Log($"没有id重复");
            }
        }

        [Button("@\"对所有\" + TTypeName + \"应用id\"", ButtonSizes.Medium)]
        private void ApplyID()
        {
            foreach (var item in TList)
            {
                item.item.id = item.id;
               UnityEditor.EditorUtility.SetDirty(item.item);
            }
            UnityEditor.AssetDatabase.SaveAssets();
            Debug.Log("已应用id");
        }
    }
}
