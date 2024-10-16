using System;

namespace MungFramework.Extension.MathExtension
{
    public static class MathExtension
    {
        /// <summary>
        /// 将int值变换delta，且在left和right之间循环
        /// left<=right
        /// </summary>
        public static void RollNum(this ref int num, int left, int right, int delta)
        {
            if (left == right)
            {
                num = left;
                return;
            }

            if (left > right)
            {
                throw new Exception("left should be less than or equal to right");
            }

            int range = right - left + 1;
            num = ((num - left)%range + delta % range + range) % range + left;
        }
        public static int Round(this float num)
        {
            return (int)Math.Round(num);
        }
    }
}
