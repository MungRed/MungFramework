﻿using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MungFramework.Logic.MungBag.FlagBag
{
    [Serializable]
    public class MungFlagBagModel : MungFramework.Model.Model
    {
        [SerializeField]
        private List<MungFlagBagItem> flagList = new();


        public ReadOnlyCollection<MungFlagBagItem> GetFlagList()
        {
            return flagList.AsReadOnly();
        }

        [Button]
        public void AddFlag(string flagName, int flagValue)
        {
            if (flagName == null)
            {
                return;
            }

            var find = FindFlag(flagName);
            if (find != null)
            {
                find.FlagValue = flagValue;
                find.LastChangeTime = DateTime.Now.ToString();
            }
            else
            {
                InsertFlag(new MungFlagBagItem()
                {
                    FlagName = flagName,
                    FlagValue = flagValue,
                    LastChangeTime = DateTime.Now.ToString()
                });
            }
        }

        public void ChangeFlagValue(string flagName, int flagValue)
        {
            if (flagName == null)
            {
                return;
            }

            var find = FindFlag(flagName);
            if (find != null)
            {
                find.FlagValue = flagValue;
                find.LastChangeTime = DateTime.Now.ToString();
            }
            else
            {
                AddFlag(flagName, flagValue);
            }
        }

        public void DeltaFlagValue(string flagName, int deltaValue)
        {
            if (flagName == null)
            {
                return;
            }

            var find = FindFlag(flagName);
            if (find != null)
            {
                find.FlagValue += deltaValue;
                find.LastChangeTime = DateTime.Now.ToString();
            }
            else
            {
                AddFlag(flagName, deltaValue);
            }
        }

        public bool RemoveFlag(string flagName)
        {
            if (flagName == null)
            {
                return false;
            }
            return flagList.RemoveAll(x => x.FlagName == flagName) > 0;
        }

        public MungFlagBagItem GetFlag(string flagName)
        {
            if (flagName == null)
            {
                return null;
            }
            return FindFlag(flagName);
        }
        public bool HaveFalg(string flagName)
        {
            if (flagName == null)
            {
                return false;
            }
            var find = FindFlag(flagName);
            return find != null;
        }

        public int GetFlagValue(string flagName)
        {
            if (flagName == null)
            {
                return 0;
            }
            var find = FindFlag(flagName);
            if (find != null)
            {
                return find.FlagValue;
            }
            return 0;
        }
        private void InsertFlag(MungFlagBagItem flag)
        {
            flagList.Add(flag);
            for (int i = flagList.Count - 1; i >= 1; i--)
            {
                if (flagList[i].FlagName.CompareTo(flagList[i - 1].FlagName) < 0)
                {
                    var temp = flagList[i];
                    flagList[i] = flagList[i - 1];
                    flagList[i - 1] = temp;
                }
            }
        }
       
        private MungFlagBagItem FindFlag(string flagName)
        {
            //二分查找
            int left = 0;
            int right = flagList.Count - 1;
            while (left <= right)
            {
                int mid = (left + right) / 2;
                if (flagList[mid].FlagName == flagName)
                {
                    return flagList[mid];
                }
                else if (flagList[mid].FlagName.CompareTo(flagName) > 0)
                {
                    right = mid - 1;
                }
                else
                {
                    left = mid + 1;
                }
            }
            return null;
        }

        [Button]
        private void SortFlagList()
        {
            flagList.Sort((x, y) => x.FlagName.CompareTo(y.FlagName));
        }
    }
}
