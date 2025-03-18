using UnityEngine;

namespace MungFramework
{
    /// <summary>
    /// 全局通用的一些静态数据，例如动画速度
    /// </summary>
    public static class StaticData
    {
        /// <summary>
        /// 用于在Update中插值
        /// </summary>
        public static float DeltaTimeLerpValue_10f
        {
            get
            {
                return Mathf.Clamp01(10 * Time.deltaTime);
            }
        }
        public static float DeltaTimeLerpValue_8f
        {
            get
            {
                return Mathf.Clamp01(8f * Time.deltaTime);
            }
        }

        public static float FixedDeltaTimeLerpValue_20f
        {
            get
            {
                return Mathf.Clamp01(20 * Time.fixedDeltaTime);
            }
        }
        public static float FixedDeltaTimeLerpValue_16f
        {
            get
            {
                return Mathf.Clamp01(16 * Time.fixedDeltaTime);
            }
        }
        public static float FixedDeltaTimeLerpValue_10f
        {
            get
            {
                return Mathf.Clamp01(10 * Time.fixedDeltaTime);
            }
        }
        public static float FixedDeltaTimeLerpValue_8f
        {
            get
            {
                return Mathf.Clamp01(8f * Time.fixedDeltaTime);
            }
        }
    }
}
