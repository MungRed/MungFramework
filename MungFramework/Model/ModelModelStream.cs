namespace MungFramework.Model
{
    /// <summary>
    /// 从输入模型到输出模型的流
    /// </summary>
    /// <typeparam name="TInputModel"></typeparam>
    /// <typeparam name="TOutputModel"></typeparam>
    public abstract class ModelModelStream<TInputModel, TOutputModel> where TInputModel : Model where TOutputModel : Model
    {

    }
}
