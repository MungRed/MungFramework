using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MungFramework.Extension.ComponentExtension
{
    public static class TransformExtension
    {
        public static Vector3 DirectionLocalPosition(this Transform trans, Vector3 localPosition)
        {
            return trans.position + trans.TransformDirection(localPosition);
        }
        public static List<GameObject> GetChildExcludeSelf(this Transform trans)
        {
            return trans.GetComponentsInChildren<Transform>().Where(x=>x!=trans).Select(x => x.gameObject).ToList();
        }
    }
}
