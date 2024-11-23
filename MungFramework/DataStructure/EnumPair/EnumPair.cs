using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace MungFramework.DataStructure.EnumPair
{
    [Serializable]
    public class EnumPair<T_EnumFather,T_EnumChild> 
        where T_EnumFather : Enum 
        where T_EnumChild : Enum
    {
        [SerializeField]
        [HideInInspector]
        private T_EnumFather enumFather;
        [SerializeField]
        [HideInInspector]
        private T_EnumChild enumChild;

        [ShowInInspector]
        public T_EnumFather EnumFather
        {
            get=> enumFather;
            set
            {
                var fatherValue = Convert.ToInt16(value);
                var childValue = Convert.ToInt16(enumChild);
                enumFather = value;
                if (fatherValue==0||childValue % fatherValue != 0)
                {
                    enumChild = default;
                }
            }
        }

        [ShowInInspector]
        public T_EnumChild EnumChild
        {
            get =>enumChild;
            set
            {
                var fatherValue = Convert.ToInt16(enumFather);
                var childValue = Convert.ToInt16(value);
                if (fatherValue!=0&&childValue % fatherValue == 0)
                {
                    enumChild = value;
                }
                else
                {
                    Debug.LogError("子枚举值应该为父枚举值的倍数");
                }
            }
        }
    }
}
