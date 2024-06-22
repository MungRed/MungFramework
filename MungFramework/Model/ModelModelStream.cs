using System;

namespace MungFramework.Model
{
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
