using UnityEngine;


namespace MungFramework.ModelData
{
    public abstract class ModelData
    {

    }

    public static class ModelDataCloneStatic
    {
        public static T Clone<T>(this T oldData) where T : ModelData
        {
            return JsonUtility.FromJson<T>(JsonUtility.ToJson(oldData));
        }
    }
}

