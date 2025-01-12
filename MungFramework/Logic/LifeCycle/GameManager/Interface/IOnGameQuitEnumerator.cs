using System.Collections;

namespace MungFramework.Logic
{
    public interface IOnGameQuitEnumerator
    {
        public IEnumerator OnGameQuitIEnumerator(GameManagerAbstract parentManager);
    }
}
