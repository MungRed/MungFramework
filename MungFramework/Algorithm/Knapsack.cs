using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MungFramework.Algorithm
{
    public static class Knapsack
    {
        /// <summary>
        /// 从itemList中选择若干物品，使总价值大于等于target且损失最小
        /// 如果全选都无法达到target，则全选
        /// 每种物品的价值是value, 数量限制是count
        /// 返回每种物品选择的数量
        /// 
        /// 数据规模：n较小，一般不超过10，value,count,target范围较大 0到INTMAX
        /// 
        /// 为避免输出复杂，itemList在传入前就按照value从小到大排序
        /// </summary>
        public static List<int> 狗粮算法(List<(int value, int count)> itemList, int target)
        {
            //贪心全选，然后从大往小依次扣
            int n = itemList.Count;
            List<int> result = itemList.Select(x => x.count).ToList();
            int sum = itemList.Sum(x => x.value * x.count);
            if (sum <= target)
            {
                return result;
            }

            for (int i = n - 1; i >= 0; i--)
            {
                int delta = sum - target;
                int reduceCount = Mathf.Min(result[i], delta / itemList[i].value);
                result[i] -= reduceCount;
                sum -= reduceCount * itemList[i].value;
            }

            return result;
        }
    }
}
