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

        [SerializeField]
        protected List<DataSOItem> DataSOItemList = new();

        public List<DataSOItem> GetAllItems()
        {
            return DataSOItemList;
        }
        public DataSOItem GetItemById(string id)
        {
            return DataSOItemList.Find(item => item.Id == id);
        }

#if UNITY_EDITOR
        private string TDataSOName => typeof(TDataSO).Name;

        [Button("@\"获取所有\" + TDataSOName + \"文件\"", ButtonSizes.Medium)]
        protected virtual void GetAllSO()
        {
            DataSOItemList.Clear();
            var guids = UnityEditor.AssetDatabase.FindAssets("t:" + typeof(TDataSO).Name);
            foreach (var guid in guids)
            {
                var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var so = UnityEditor.AssetDatabase.LoadAssetAtPath<TDataSO>(path);
                DataSOItemList.Add(new(so.Id, so));
            }
            SortList();
        }
        [Button("将所有id设置为资源名称", ButtonSizes.Medium)]
        private void SetAllIdWithName()
        {
            foreach (var dataSOItem in DataSOItemList)
            {
                dataSOItem.Id = dataSOItem.Item.name;
            }
            Debug.Log("已设置");
        }

        [Button("将所有资源名称设置为id", ButtonSizes.Medium)]
        private void SetAllNameWithId()
        {
            foreach (var dataSOItem in DataSOItemList)
            {
                UnityEditor.AssetDatabase.RenameAsset(UnityEditor.AssetDatabase.GetAssetPath(dataSOItem.Item), dataSOItem.Id);
                UnityEditor.EditorUtility.SetDirty(dataSOItem.Item);
            }
            Debug.Log("已设置");
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
            foreach (var dataSOItem in DataSOItemList)
            {
                if (dataSOItem.Id.Contains(" "))
                {
                    Debug.LogError($"id包含空格(已经清除)：id:{dataSOItem.Id} name:{dataSOItem.Item.name}");
                    dataSOItem.Id = dataSOItem.Id.Replace(" ", "");
                }
                if (idSet.Contains(dataSOItem.Id))
                {
                    flag = false;
                    Debug.LogError($"id重复：id:{dataSOItem.Id} name:{dataSOItem.Item.name}");
                }
                idSet.Add(dataSOItem.Id);
            }

            if (flag)
            {
                Debug.Log($"没有id重复");
            }
        }

        [Button("@\"对所有\" + TDataSOName + \"应用id\"", ButtonSizes.Medium)]
        private void ApplyID()
        {
            foreach (var dataSOItem in DataSOItemList)
            {
                dataSOItem.Item.Id = dataSOItem.Id;
                UnityEditor.EditorUtility.SetDirty(dataSOItem.Item);
            }
            UnityEditor.AssetDatabase.SaveAssets();
            Debug.Log("已应用id");
        }
#endif
    }
}
