using System;

namespace MungFramework.Model
{
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
