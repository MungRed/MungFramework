using MungFramework.Logic.Input;
using System;

namespace MungFramework.Logic.FSM
{

    /// <summary>
    /// 有限状态机状态接口
    /// </summary>
    public interface IFSMState<StateEnum,Parameter> where StateEnum : Enum where Parameter : IFSMParameter
    {

        /// <summary>
        /// 进入状态
        /// </summary>
        public bool OnStateEnter(StateEnum lastState, Parameter parameter);

        /// <summary>
        /// 离开状态
        /// </summary>
        public bool OnStateExit(StateEnum nextState, Parameter parameter);

        /// <summary>
        /// 帧更新,返回下一状态
        /// </summary>
        public StateEnum OnStateUpdate(Parameter parameter);
        /// <summary>
        /// 固定帧更新,返回下一状态
        /// </summary>
        public StateEnum OnStateFixedUpdate(Parameter parameter);
        /// <summary>
        /// 按键输入,返回下一状态
        /// </summary>
        public StateEnum OnStateInput(InputValueEnum inputValue, Parameter parameter);
    }
}
