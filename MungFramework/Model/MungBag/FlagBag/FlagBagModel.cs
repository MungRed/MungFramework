using Sirenix.OdinInspector;
using System;

namespace MungFramework.Model.MungBag.FlagBag
{
    [Serializable]
    public class FlagBagModel : ListBagModel<FlagBagItem>
    {
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
                InsertFlag(new FlagBagItem()
                {
                    FlagName = flagName,
                    FlagValue = flagValue,
                    LastChangeTime = DateTime.Now.ToString()
                });
            }
        }

        public void ChangeFlagValue(string flagName, int flagValue)
        {
            AddFlag(flagName, flagValue);
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
            return ItemList.RemoveAll(x => x.FlagName == flagName) > 0;
        }

        public FlagBagItem GetFlag(string flagName)
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

        private void InsertFlag(FlagBagItem flag)
        {
            ItemList.Add(flag);
            for (int i = ItemList.Count - 1; i >= 1; i--)
            {
                if (ItemList[i].FlagName.CompareTo(ItemList[i - 1].FlagName) < 0)
                {
                    Algorithm.Math.Swap(ItemList[i - 1], ItemList[i]);
                }
            }
        }

        private FlagBagItem FindFlag(string flagName)
        {
            //二分查找
            int left = 0;
            int right = ItemList.Count - 1;
            while (left <= right)
            {
                int mid = (left + right) / 2;
                if (ItemList[mid].FlagName == flagName)
                {
                    return ItemList[mid];
                }
                else if (ItemList[mid].FlagName.CompareTo(flagName) > 0)
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

#if UNITY_EDITOR
        [Button]
        private void SortFlagList()
        {
            ItemList.Sort((x, y) => x.FlagName.CompareTo(y.FlagName));
        }
#endif
    }

}
