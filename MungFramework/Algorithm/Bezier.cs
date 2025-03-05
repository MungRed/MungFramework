using UnityEngine;

namespace MungFramework.Algorithm.Bezier
{
    public static class Bezier
    {
        public static Vector3[] GetBezierPointList(Vector3 startPosition, Vector3 endPosition, Vector3 midPosition, int segmentCount, int left, int right)
        {
            Vector3[] result = new Vector3[right - left + 1];
            for (int i = left; i <= right; i++)
            {
                float t = (float)i / segmentCount;
                Vector3 point = GetBezierPoint(startPosition, endPosition, midPosition, t);
                result[i - left] = point;
            }
            return result;
        }

        public static Vector3 GetBezierPoint(Vector3 startPosition, Vector3 endPosition, Vector3 midPosition, float t)
        {
            var p1 = Vector3.Lerp(startPosition, midPosition, t);
            var p2 = Vector3.Lerp(midPosition, endPosition, t);
            return Vector3.Lerp(p1, p2, t);
        }
    }
}
