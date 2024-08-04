using MungFramework.Logic;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace MungFramework.Extension.CoroutineExtension
{
    public static class IEnumeratorExtension
    {
        public static Coroutine StartCoroutine(this IEnumerator enumerator)
        {
            return GameApplicationAbstract.Instance.StartCoroutine(enumerator);
        }

    }

}
