using System;

namespace MungFramework.Model
{
    /// <summary>
    /// 枚举的映射
    /// </summary>
    public abstract class EnumMap<T_InputEnum,T_OutputEnum> where T_InputEnum : Enum where T_OutputEnum : Enum
    {
        public abstract T_OutputEnum Map(T_InputEnum input);
    }
    public abstract class EnumMap<T_InputEnum, T_OutputEnum,T_Parameter> where T_InputEnum : Enum where T_OutputEnum : Enum
    {
        public abstract T_OutputEnum Map(T_InputEnum input, T_Parameter parameter);
    }
}
