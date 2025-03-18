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
        public void OnInput(InputValueEnum inputType, InputKeyEnum inputKey);
    }

}
