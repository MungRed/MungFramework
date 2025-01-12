using System.Collections;

namespace MungFramework.Logic
{
    /// <summary>
    /// 实现该接口的GameManager会在游戏开始时异步执行
    /// </summary>
    public interface IOnGameStartEnumerator
    {
        public IEnumerator OnGameStartIEnumerator(GameManagerAbstract parentManager);
    }
}
