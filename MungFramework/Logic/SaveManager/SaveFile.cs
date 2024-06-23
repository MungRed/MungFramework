using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace MungFramework.Logic.Save
{
    [Serializable]
    public class SaveFile : MungFramework.Model.Model
    {
        [SerializeField]
        //[ReadOnly]
        public string SaveName;

        [SerializeField]
        //[ReadOnly]
        private SerializedDictionary<string, string> DataDictionary;

        public SaveFile(string saveName, List<KeyValuePair<string, string>> datas)
        {
            SaveName = saveName;
            DataDictionary = new();
            foreach (var data in datas)
            {
                DataDictionary.Add(data.Key, data.Value);
            }
        }

        public bool HasKey(string key)
        {
            return DataDictionary.ContainsKey(key);
        }
        public bool DeleteKey(string key)
        {
            return DataDictionary.Remove(key);
        }


        public bool SetValue(string key, string value)
        {
            DataDictionary[key] = value;
            return true;
        }
        public (string val, bool hasVal) GetValue(string key)
        {
            if (HasKey(key))
            {
                return (DataDictionary[key], true);
            }
            return ("", false);
        }

        public List<KeyValuePair<string, string>> GetKeyValues()
        {
            return DataDictionary.ToList();
        }

    }
}
