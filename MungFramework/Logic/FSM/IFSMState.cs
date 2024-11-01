using MungFramework.Logic.Input;
using System;

namespace MungFramework.Logic.FSM
{
    /// <summary>
    /// 有限状态机状态接口
    /// </summary>
    public interface IFSMState<T_StateEnum, T_Parameter> where T_StateEnum : Enum where T_Parameter : IFSMParameter
    {
        /// <summary>
        /// 进入状态
        /// </summary>
        public bool OnStateEnter(T_StateEnum lastState, T_Parameter parameter);
        /// <summary>
        /// 离开状态
        /// </summary>
        public bool OnStateExit(T_StateEnum nextState, T_Parameter parameter);
        /// <summary>
        /// 帧更新,返回下一状态
        /// </summary>
        public T_StateEnum OnStateUpdate(T_Parameter parameter);
        /// <summary>
        /// 固定帧更新,返回下一状态
        /// </summary>
        public T_StateEnum OnStateFixedUpdate(T_Parameter parameter);

        /// <summary>
        /// 延迟更新,返回下一状态
        /// </summary>
        public T_StateEnum OnStateLateUpdate(T_Parameter parameter);
        /// <summary>
        /// 按键输入,返回下一状态
        /// </summary>
        public T_StateEnum OnStateInput(InputValueEnum inputValue, T_Parameter parameter);
    }
}
