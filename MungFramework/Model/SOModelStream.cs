﻿using System;
namespace MungFramework.Model
{
    /// <summary>
    /// 从SO到数据模型的流
    /// </summary>
    /// <typeparam name="TInputSO"></typeparam>
    /// <typeparam name="TOutputModel"></typeparam>
    public abstract class SOModelStream<TInputSO, TOutputModel> where TInputSO : ScriptableObjects.DataSO where TOutputModel : Model
    {
        private Func<TInputSO, TOutputModel> Process;

        public void SetProcess(Func<TInputSO, TOutputModel> process)
        {
            Process = process;
        }
        public Func<TInputSO, TOutputModel> GetProcess()
        {
            return Process;
        }
    }
}
