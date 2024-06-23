using System;
namespace MungFramework.Model
{
    /// <summary>
    /// 从SO到数据模型的流
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public abstract class SOModelStream<TInput, TOutput> where TInput : ScriptableObjects.DataSO where TOutput : Model
    {
        private Func<TInput, TOutput> Process;

        public void SetProcess(Func<TInput, TOutput> process)
        {
            Process = process;
        }
        public Func<TInput, TOutput> GetProcess()
        {
            return Process;
        }
    }
}
