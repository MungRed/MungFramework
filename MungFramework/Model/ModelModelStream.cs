using System;

namespace MungFramework.Model
{
    /// <summary>
    /// 从输入模型到输出模型的流
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public abstract class ModelModelStream<TInput, TOutput> where TInput : Model where TOutput : Model
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
