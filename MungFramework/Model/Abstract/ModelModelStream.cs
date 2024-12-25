namespace MungFramework.Model
{
    /// <summary>
    /// 从输入模型到输出模型的流
    /// </summary>
    public abstract class ModelModelStream<T_InputModel, T_OutputModel> where T_InputModel : Model where T_OutputModel : Model
    {
        public abstract T_OutputModel Stream(T_InputModel inputModel);
    }

    public abstract class ModelModelStream<T_InputModel, T_OutputModel, T_Parameter> where T_InputModel : Model where T_OutputModel : Model
    {
        public abstract T_OutputModel Stream(T_InputModel inputModel, T_Parameter parameter);
    }
}
