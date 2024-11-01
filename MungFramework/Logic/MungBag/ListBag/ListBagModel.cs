using System;
using System.Collections.Generic;
using UnityEngine;

namespace MungFramework.Logic.MungBag
{
    [Serializable]
    public class ListBagModel<T_ItemType>:MungFramework.Model.Model
    {
        [SerializeField]
        private List<T_ItemType> itemList = new();
        public List<T_ItemType> ItemList=> itemList;
    }
}
