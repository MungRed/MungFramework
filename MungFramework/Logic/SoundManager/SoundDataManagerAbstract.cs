using System.Collections;
using UnityEngine;

namespace MungFramework.Logic.Sound
{
    public abstract class SoundDataManagerAbstract : SavableGameManagerAbstract
    {
        public override IEnumerator Load()
        {
            yield return null;
        }

        public override IEnumerator Save()
        {
            yield return null;
        }
    }
}