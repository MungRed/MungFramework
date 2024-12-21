using Sirenix.OdinInspector;
using System;

namespace MungFramework.DataStructure.FlagEnum
{
    /// <summary>
    /// Flag枚举
    /// 用于表示多种状态
    /// 
    /// 举例：
    /// 假设角色可以从Move和Action和Battle之间切换，只有在Move的时候可以移动
    /// 当角色进入对话状态，可以转换到Action，对话结束后转回Move
    /// 当角色进入战斗状态，可以转换到Battle，战斗结束后转回Move
    /// 但会有一个问题，当角色进入对话状态再进入战斗状态，战斗结束后转回Move，导致之前的对话状态丢失
    /// 所以可以通过FlagEnum来解决这个问题
    /// 将状态转移变成添加删除Flag的方式，这样可以保留之前的状态
    /// </summary>
    [Serializable]
    public class FlagEnum<T> where T : Enum
    {
        [ShowInInspector]
        public T Value
        {
            get;
            private set;
        }

        private int ToInt(T value) => (int)(object)value;
        private T ToEnum(int value) => (T)(object)value;

        public FlagEnum<T> AddFlag(T value)
        {
            Value = ToEnum(ToInt(Value) | ToInt(value));
            return this;
        }
        public FlagEnum<T> RemoveFlag(T flag)
        {
            Value = ToEnum(ToInt(Value) & ~ToInt(flag));
            return this;
        }
        public FlagEnum<T> ClearFlag()
        {
            Value = ToEnum(0);
            return this;
        }
        public FlagEnum<T> SetFlag(T value)
        {
            Value = value;
            return this;
        }
        public bool HasFlag(T flag)
        {
            return (ToInt(Value) & ToInt(flag)) != 0;
        }
        public bool HasFlag(params T[] flag)
        {
            foreach (var f in flag)
            {
                if (!HasFlag(f))
                {
                    return false;
                }
            }
            return true;
        }
        public bool Equals(T value)
        {
            return Value.Equals(value);
        }
    }
}
