using System.Collections;

namespace MungFramework.Logic
{
    public interface IOnGameQuitIEnumerator
    {
        public IEnumerator OnGameQuitIEnumerator(GameManagerAbstract parentManager);
    }
}
