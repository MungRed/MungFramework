namespace MungFramework.Logic.Input
{
    /// <summary>
    /// 输入接收器接口
    /// </summary>
    public interface IInputAcceptor
    {
        public InputManagerAbstract InputManager 
        {
            get;
        }

        /// <summary>
        /// 输入事件
        /// </summary>
        public void OnInput(InputValueEnum inputType);
    }

}
