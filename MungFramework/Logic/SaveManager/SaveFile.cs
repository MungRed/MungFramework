using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;


namespace MungFramework.Logic.Save
{
    [Serializable]
    public class SaveFile : Model.Model
    {
        [SerializeField]
        public string SaveName;
        [SerializeField]
        public string SaveTime;

        [SerializeField]
        public SerializedDictionary<string, string> DataDictionary = new();



        public bool HasKey(string key)
        {
            return DataDictionary.ContainsKey(key);
        }
        public bool RemoveKey(string key)
        {
            return DataDictionary.Remove(key);
        }


        public bool SetValue(string key, string value)
        {
            DataDictionary[key] = value;
            return true;
        }
        public (string value, bool hasValue) GetValue(string key)
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
        public void SetKeyValues(List<KeyValuePair<string, string>> keyvalues)
        {
            DataDictionary.Clear();
            foreach (var keyvalue in keyvalues)
            {
                DataDictionary.Add(keyvalue.Key, keyvalue.Value);
            }
        }

        public void Clear()
        {
            DataDictionary.Clear();
        }
    }
}
