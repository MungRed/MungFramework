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
        public static float DeltaTimeLerpValue_Bigger
        {
            get
            {
                return Mathf.Clamp01(10 * Time.deltaTime);
            }
        }
        public static float DeltaTimeLerpValue_Smaller
        {
            get
            {
                return Mathf.Clamp01(8f * Time.deltaTime);
            }
        }
        public static float FixedDeltaTimeLerpValue_Bigger
        {
            get
            {
                return Mathf.Clamp01(10 * Time.fixedDeltaTime);
            }
        }
        public static float FixedDeltaTimeLerpValue_Smaller
        {
            get
            {
                return Mathf.Clamp01(8f * Time.fixedDeltaTime);
            }
        }
    }
}
