using System.Collections;

namespace MungFramework.Logic
{
    /// <summary>
    /// 实现该接口的GameManager会在场景加载时异步执行
    /// 可以用于存档系统等需要异步假装的游戏管理器
    /// </summary>
    public interface IOnSceneLoadIEnumerator
    {
        public IEnumerator OnSceneLoadIEnumerator(GameManagerAbstract parentManager);
    }
}
