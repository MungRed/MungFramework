using MungFramework.Logic;
using System.Collections;
using UnityEngine;

namespace MungFramework.Extension.CoroutineExtension
{
    public static class IEnumeratorExtension
    {
        public static Coroutine StartCoroutine(this IEnumerator enumerator)
        {
            return GameApplicationAbstract.Instance.StartCoroutine(enumerator);
        }
        public static void StopCoroutine(this Coroutine coroutine)
        {
            GameApplicationAbstract.Instance.StopCoroutine(coroutine);
        }
    }
}
