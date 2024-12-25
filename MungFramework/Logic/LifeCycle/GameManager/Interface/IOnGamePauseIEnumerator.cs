using System.Collections;

namespace MungFramework.Logic
{
    public interface IOnGamePauseIEnumerator
    {
        public IEnumerator OnGamePauseIEnumerator(GameManagerAbstract parentManager);
    }
}
