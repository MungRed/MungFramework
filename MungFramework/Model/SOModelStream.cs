namespace MungFramework.Model
{
    /// <summary>
    /// 从SO到数据模型的流
    /// </summary>
    /// <typeparam name="T_InputSO"></typeparam>
    /// <typeparam name="T_OutputModel"></typeparam>
    public abstract class SOModelStream<T_InputSO, T_OutputModel> where T_InputSO : ScriptableObjects.DataSO where T_OutputModel : Model
    {
        public abstract T_OutputModel Stream(T_InputSO so);
    }

    public abstract class SOModelStream<T_InputSO, T_OutputModel,T_Parameter> where T_InputSO : ScriptableObjects.DataSO where T_OutputModel : Model
    {
        public abstract T_OutputModel Stream(T_InputSO so,T_Parameter parameter);
    }
}
