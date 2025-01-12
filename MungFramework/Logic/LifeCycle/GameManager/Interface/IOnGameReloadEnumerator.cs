using System.Collections;

namespace MungFramework.Logic
{
    public interface  IOnGameReloadEnumerator
    {
        public IEnumerator OnGameReloadIEnumerator(GameManagerAbstract parentManager);
    }
}
