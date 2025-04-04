using UnityEngine;

namespace MungFramework.Extension.VectorExtension
{
    public static class VectorExtension
    {
        public static bool IsZeroDistance(this Vector3 vec)
        {
            return vec.sqrMagnitude < 0.01f;
        }
    }
}
