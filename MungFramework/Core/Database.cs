using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace MungFramework.Core
{
    public static class Database
    {
        [Serializable]
        public class DataTable
        {
            public string TableName;
            public string TableTime;

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
                    DataDictionary.Add(keyvalue.Key, keyvalue.Value);
                }
            }
        }

        public static string DatabasePath => Application.dataPath + "/data";
        public static string DataTableFormat => "sav";


        /// <summary>
        /// 是否存在数据库
        /// </summary>
        public static bool ExistDatabase()
        {
            //Debug.Log("检查数据库是否存在" + DatabasePath);

            //检查路径是否存在
            if (!FileSystem.HaveDirectory(DatabasePath))
            {
                Debug.Log("路径不存在" + DatabasePath);
                return false;
            }

            //检查系统文件是否存在
            if (!FileSystem.HaveFile(DatabasePath, "system", DataTableFormat))
            {
                Debug.Log("系统文件不存在" + DatabasePath + "/system." + DataTableFormat);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 是否存在数据表
        /// </summary>
        public static bool ExistDataTable(string tableName)
        {
            return FileSystem.HaveFile(DatabasePath, tableName, DataTableFormat);
        }

        /// <summary>
        /// 创建数据库（会删除原有数据库）
        /// </summary>
        public static IEnumerator CreateDatabase()
        {
            FileSystem.DeleteDirectory(DatabasePath);
            Directory.CreateDirectory(DatabasePath);

            //创建系统文件
            string systemTableName = "system";

            DataTable dataTable = new DataTable()
            {
                TableName = systemTableName,
                TableTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            //异步写入文件
            yield return FileSystem.WriteFileAsync(DatabasePath, systemTableName, DataTableFormat, JsonUtility.ToJson(dataTable));
        }

        /// <summary>
        /// 获取数据表
        /// </summary>
        private static IEnumerator GetDataTable(string tableName, UnityAction<DataTable> reasultAction)
        {
            //检查数据库是否存在
            if (!ExistDatabase())
            {
                Debug.LogError("数据库不存在， 获取数据表失败" + tableName);
                yield break;
            }

            string readContent = null;

            yield return FileSystem.ReadFileAsync(DatabasePath, tableName, DataTableFormat, x => { readContent = x; });

            if (readContent == null)
            {
                Debug.Log("获取数据表失败" + tableName);
                yield break;
            }

            try
            {
                DataTable dataTable = JsonUtility.FromJson<DataTable>(readContent);
                reasultAction.Invoke(dataTable);
                yield break;
            }
            catch (Exception e)
            {
                Debug.LogError("解析数据表失败" + tableName + e.Message);
                yield break;
            }
        }

        /// <summary>
        /// 移除数据表
        /// </summary>
        public static bool RemoveDataTable(string tableName)
        {
            if (!ExistDatabase())
            {
                Debug.LogError("数据库不存在， 删除数据表失败" + tableName);
                return false;
            }

            return FileSystem.DeleteFile(DatabasePath, tableName, DataTableFormat);
        }


        /// <summary>
        /// 获得数据表的键值对
        /// </summary>
        public static IEnumerator GetKeyValues(string tableName, UnityAction<List<KeyValuePair<string, string>>, string> resultAction)
        {
            DataTable dataTable = null;
            yield return GetDataTable(tableName, x => dataTable = x);

            if (dataTable == null)
            {
                yield break;
            }
            resultAction.Invoke(dataTable.GetKeyValues(), dataTable.TableTime);
        }

        /// <summary>
        /// 设置数据表的键值对
        /// </summary>
        public static IEnumerator SetKeyValues(string tableName, List<KeyValuePair<string, string>> keyValues)
        {
            DataTable dataTable = new()
            {
                TableName = tableName,
                TableTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };
            dataTable.SetKeyValues(keyValues);
            yield return FileSystem.WriteFileAsync(DatabasePath, tableName, DataTableFormat, JsonUtility.ToJson(dataTable, true));
        }



    }
}
