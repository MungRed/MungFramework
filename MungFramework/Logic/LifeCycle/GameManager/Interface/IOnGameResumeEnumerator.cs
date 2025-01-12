using System.Collections;

namespace MungFramework.Logic
{
    public interface IOnGameResumeEnumerator
    {
        public IEnumerator OnGameResumeIEnumerator(GameManagerAbstract parentManager);
    }
}
