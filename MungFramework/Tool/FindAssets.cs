#if UNITY_EDITOR
using System;
using System.Collections.Generic;

namespace MungFramework.Tool
{
    public static class FindAssets
    {
        public static IEnumerable<T> Find<T>(string name) where T : UnityEngine.Object
        {
            var guids = UnityEditor.AssetDatabase.FindAssets($"t:{typeof(T).Name} {name}");
            foreach (var guid in guids)
            {
                var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var go = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
                yield return go;
            }
        }
        public static IEnumerable<T> Find<T>(Func<T, bool> select) where T : UnityEngine.Object
        {
            var guids = UnityEditor.AssetDatabase.FindAssets($"t:{typeof(T).Name}");
            foreach (var guid in guids)
            {
                var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var go = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
                if (select(go))
                {
                    yield return go;
                }
            }
        }

        public static T FindFirst<T>(string name) where T : UnityEngine.Object
        {
            var guids = UnityEditor.AssetDatabase.FindAssets($"t:{typeof(T).Name} {name}");
            foreach (var guid in guids)
            {
                var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var go = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
                return go;
            }
            return null;
        }

        public static T FindFirst<T>(Func<T, bool> select) where T : UnityEngine.Object
        {
            var guids = UnityEditor.AssetDatabase.FindAssets($"t:{typeof(T).Name}");
            foreach (var guid in guids)
            {
                var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var go = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
                if (select(go))
                {
                    return go;
                }
            }
            return null;
        }

        public static T FindFirst<T>() where T : UnityEngine.Object
        {
            var guids = UnityEditor.AssetDatabase.FindAssets($"t:{typeof(T).Name}");
            foreach (var guid in guids)
            {
                var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var go = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
                return go;
            }
            return null;
        }
        public static IEnumerable<T> Find<T>() where T : UnityEngine.Object
        {
            var guids = UnityEditor.AssetDatabase.FindAssets($"t:{typeof(T).Name}");
            foreach (var guid in guids)
            {
                var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var go = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
                yield return go;
            }
        }

    }
}
#endif
