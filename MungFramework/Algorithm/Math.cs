using System;

namespace MungFramework.Algorithm
{
    public static class Math
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
            num = ((num - left) % range + delta % range + range) % range + left;
        }
        public static void Clamp<T>(this ref T num, T min, T max) where T : struct, IComparable<T>
        {
            if (num.CompareTo(min) < 0)
            {
                num = min;
            }
            else if (num.CompareTo(max) > 0)
            {
                num = max;
            }
        }
        public static void Swap<T>(ref T a, ref T b) where T : struct
        {
            T temp = a;
            a = b;
            b = temp;
        }
        public static void Swap<T>(T a, T b) where T : class
        {
            T temp = a;
            a = b;
            b = temp;
        }
        public static int Round(this float num)
        {
            return (int)System.Math.Round(num);
        }
    }
}
