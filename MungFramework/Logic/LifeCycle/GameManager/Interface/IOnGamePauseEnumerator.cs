using System.Collections;

namespace MungFramework.Logic
{
    public interface IOnGamePauseEnumerator
    {
        public IEnumerator OnGamePauseIEnumerator(GameManagerAbstract parentManager);
    }
}
