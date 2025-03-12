using System;
using UnityEngine;

namespace MungFramework.Model.UiData
{
    public interface IUiDataService<T_Enum> where T_Enum : Enum
    {
        public abstract string GetTextData(T_Enum key);
        public abstract Sprite GetSpriteData(T_Enum key);
    }

    public interface IUiDataService<T_Enum, T_Parameter> where T_Enum : Enum
    {
        public abstract string GetTextData(T_Enum key, T_Parameter parameter);
        public abstract Sprite GetSpriteData(T_Enum key, T_Parameter parameter);
    }
}
