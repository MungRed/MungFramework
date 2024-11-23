using System;
using System.Collections.Generic;
using UnityEngine;

namespace MungFramework.Logic.MungBag
{
    [Serializable]
    public class ListBagModel<T_ItemType> : MungFramework.Model.Model
    {
        [SerializeField]
        private List<T_ItemType> itemList = new();
        public List<T_ItemType> ItemList => itemList;

        public void Swap(int index1, int index2)
        {
            if (index1 < 0 || index1 >= itemList.Count || index2 < 0 || index2 >= itemList.Count)
            {
                return;
            }
            var temp = itemList[index1];
            itemList[index1] = itemList[index2];
            itemList[index2] = temp;
        }
    }
}
