using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

namespace MungFramework.DataStructure.FlagEnum
{
    [Serializable]
    public class FlagEnumSet<T> where T : Enum
    {
        [ShowInInspector]
        private Dictionary<T, int> flags = new();

        public FlagEnumSet<T> AddFlag(T flag)
        {
            if (flags.ContainsKey(flag))
            {
                flags[flag]++;
            }
            else
            {
                flags.Add(flag, 1);
            }
            return this;
        }
        public FlagEnumSet<T> RemoveFlag(T flag)
        {
            if (flags.ContainsKey(flag))
            {
                flags[flag]--;
                if (flags[flag] <= 0)
                {
                    flags.Remove(flag);
                }
            }
            return this;
        }
        public bool HasFlag(T flag)
        {
            return flags.ContainsKey(flag);
        }
        public bool OnlyFlag(T flag)
        {
            return flags.Count == 1 && flags.ContainsKey(flag);
        }
        public bool Empty()
        {
            return flags.Count == 0;
        }

        public FlagEnumSet<T> ClearFlag()
        {
            flags.Clear();
            return this;
        }

    }
}
