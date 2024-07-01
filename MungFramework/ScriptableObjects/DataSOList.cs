using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MungFramework.ScriptableObjects
{
    /// <summary>
    /// ScriptableObject列表基类
    /// </summary>
    /// <typeparam name="TDataSO"></typeparam>
    public abstract class DataSOList<TDataSO> : ScriptableObject where TDataSO : DataSO
    {
        [Serializable]
        public class DataSOItem
        {
            public string Id;
            public TDataSO Item;

            public DataSOItem(string id, TDataSO item)
            {
                Id = id;
                Item = item;
            }
        }

        private string TDataSOName => typeof(TDataSO).Name;

        [SerializeField]
        protected List<DataSOItem> DataSOItemList = new();

        [Button("@\"获取所有\" + TDataSOName + \"文件\"", ButtonSizes.Medium)]
        private void GetAllSO()
        {
            DataSOItemList.Clear();
            var guids = UnityEditor.AssetDatabase.FindAssets("t:" + typeof(TDataSO).Name);
            foreach (var guid in guids)
            {
                var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var so = UnityEditor.AssetDatabase.LoadAssetAtPath<TDataSO>(path);
                DataSOItemList.Add(new(so.Id,so));
            }
            SortList();
        }

        [Button("对id排序", ButtonSizes.Medium)]
        private void SortList()
        {
            DataSOItemList.Sort((a, b) => string.Compare(a.Id, b.Id, StringComparison.Ordinal));
            Debug.Log("已排序");
        }

        [Button("检查id重复性和正确性", ButtonSizes.Medium)]
        private void CheckID()
        {
            bool flag = true;
            HashSet<string> idSet = new HashSet<string>();
            foreach (var t in DataSOItemList)
            {
                if (t.Id.Contains(" "))
                {
                    Debug.LogError($"id包含空格(已经清除)：id:{t.Id} name:{t.Item.name}");
                    t.Id = t.Id.Replace(" ", "");
                }
                if (idSet.Contains(t.Id))
                {
                    flag = false;
                    Debug.LogError($"id重复：id:{t.Id} name:{t.Item.name}");
                }
                idSet.Add(t.Id);
            }

            if (flag)
            {
                Debug.Log($"没有id重复");
            }
        }

        [Button("@\"对所有\" + TDataSOName + \"应用id\"", ButtonSizes.Medium)]
        private void ApplyID()
        {
            foreach (var item in DataSOItemList)
            {
                item.Item.Id = item.Id;
               UnityEditor.EditorUtility.SetDirty(item.Item);
            }
            UnityEditor.AssetDatabase.SaveAssets();
            Debug.Log("已应用id");
        }
    }
}
