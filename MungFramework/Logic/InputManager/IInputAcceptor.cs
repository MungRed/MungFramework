namespace MungFramework.Logic.Input
{
    /// <summary>
    /// 输入接收器接口
    /// </summary>
    public interface IInputAcceptor
    {
        /// <summary>
        /// 输入事件
        /// </summary>
        /// <param name="inputType"></param>
        public void OnInput(InputTypeValue inputType);
    }

}
