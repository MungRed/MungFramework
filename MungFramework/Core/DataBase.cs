using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Windows;

namespace MungFramework.Core
{
    public static class DataBase
    {
        [Serializable]
        public class DataTable
        {
            public string TableName;
            [SerializeField]
            public SerializedDictionary<string, string> DataDictionary;

            public List<KeyValuePair<string, string>> GetKeyValues()
            {
                if (DataDictionary == null)
                {
                    return new();
                }
                return DataDictionary.ToList();
            }
            public void SetKeyValues(List<KeyValuePair<string, string>> keyvalues)
            {
                if (DataDictionary == null)
                {
                    DataDictionary = new();
                }

                DataDictionary.Clear();
                foreach (var keyvalue in keyvalues)
                {
                    DataDictionary.Add(keyvalue.Key,keyvalue.Value);
                }
            }
        }

        private static string DataBasePath => Application.dataPath + "/data";
        private static string DataTableFormat => "sav";


        /// <summary>
        /// 是否存在数据库
        /// </summary>
        /// <returns></returns>
        public static bool ExistDataBase()
        {
            Debug.Log("检查数据库是否存在" + DataBasePath);

            //检查路径是否存在
            if (!FileSystem.HasDirectory(DataBasePath))
            {
                Debug.Log("路径不存在" + DataBasePath);
                return false;
            }

            //检查系统文件是否存在
            if (!FileSystem.HasFile(DataBasePath, "system", DataTableFormat))
            {
                Debug.Log("系统文件不存在" + DataBasePath + "/system." + DataTableFormat);
                return false;
            }

            return true;
        }
        /// <summary>
        /// 创建数据库（会删除原有数据库）
        /// </summary>
        public static bool CreateDataBase()
        {
            //删除数据库
            if (Directory.Exists(DataBasePath))
            {
                Directory.Delete(DataBasePath);
            }

            Directory.CreateDirectory(DataBasePath);

            //创建系统文件
            string systemTableName = "system";
            DataTable dataTable = new DataTable();
            dataTable.TableName = systemTableName;

            return FileSystem.WriteFile(DataBasePath, systemTableName, DataTableFormat, JsonUtility.ToJson(dataTable));
        }
        /// <summary>
        /// 获取数据表
        /// </summary>
        private static (DataTable, bool) GetDataTable(string tableName)
        {
            //检查数据库是否存在
            if (!ExistDataBase())
            {
                Debug.Log("数据库不存在， 获取数据表失败" + tableName);
                return (null, false);
            }

            var (content, success) = FileSystem.ReadFile(DataBasePath, tableName, DataTableFormat);

            if (success == false)
            {
                Debug.Log("获取数据表失败" + tableName);
                return (null, false);
            }

            DataTable dataTable = JsonUtility.FromJson<DataTable>(content);
            return (dataTable, true);
        }
        /// <summary>
        /// 移除数据表
        /// </summary>
        public static bool RemoveDataTable(string tableName)
        {
            if (!ExistDataBase())
            {
                Debug.Log("数据库不存在， 删除数据表失败" + tableName);
                return false;
            }

            return FileSystem.DeleteFile(DataBasePath, tableName, DataTableFormat);
        }
        /// <summary>
        /// 获得数据表的键值对
        /// </summary>
        public static (List<KeyValuePair<string, string>>, bool) GetKeyValues(string tableName)
        {
            var (dataTable, success) = GetDataTable(tableName);
            if (success == false)
            {
                return (null, false);
            }
            return (dataTable.GetKeyValues(), true);
        }
        /// <summary>
        /// 设置数据表的键值对
        /// </summary>
        public static void SetKeyValues(string tableName, List<KeyValuePair<string, string>> keyValues)
        {
            DataTable dataTable = new();
            dataTable.TableName = tableName;
            dataTable.SetKeyValues(keyValues);

            FileSystem.WriteFile(DataBasePath, tableName, DataTableFormat, JsonUtility.ToJson(dataTable,true));
        }
    }
}
