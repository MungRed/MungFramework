using MungFramework.Logic.Input;
using System;

namespace MungFramework.Logic.FSM
{
    /// <summary>
    /// 有限状态机管理器接口
    /// </summary>
    public interface  IFSMManager<StateEnum, Parameter> where StateEnum : Enum where Parameter : IFSMParameter
    {
        public IFSMManager<StateEnum, Parameter> FSManager
        {
            get;
        }
        public Parameter FSMParameter
        {
            get;
        }

        /// <summary>
        /// 当前状态
        /// </summary>
        public StateEnum FSMNowState
        {
            set; get;
        }

        /// <summary>
        /// 获取每个状态对应的实例
        /// 约束：每个状态只能有一个实例
        /// </summary>
        public IFSMState<StateEnum, Parameter> FSMGetStateInstance(StateEnum state);

        /// <summary>
        /// 帧更新
        /// </summary>
        public void FSMUpdate()
        {
            IFSMState<StateEnum, Parameter> state = FSMGetStateInstance(FSMNowState);
            if (state == null)
            {
                return;
            }

            StateEnum nextState = state.OnStateUpdate(FSMParameter);
            if (!nextState.Equals(FSMNowState))
            {
                FSMChangeState(nextState);
            }
        }

        /// <summary>
        /// 固定帧更新
        /// </summary>
        public void FSMFixedUpdate()
        {
            IFSMState<StateEnum, Parameter> state = FSMGetStateInstance(FSMNowState);
            if (state == null)
            {
                return;
            }

            StateEnum nextState = state.OnStateFixedUpdate(FSMParameter);
            if (!nextState.Equals(FSMNowState))
            {
                FSMChangeState(nextState);
            }
        }
        /// <summary>
        /// 按键输入
        /// </summary>
        public void FSMInput(InputValueEnum inputValue)
        {
            IFSMState<StateEnum, Parameter> state = FSMGetStateInstance(FSMNowState);
            if (state == null)
            {
                return;
            }

            StateEnum nextState = state.OnStateInput(inputValue,FSMParameter);
            if (!nextState.Equals(FSMNowState))
            {
                FSMChangeState(nextState);
            }
        }

        /// <summary>
        /// 改变状态
        /// </summary>
        public void FSMChangeState(StateEnum nextState)
        {
            //当前状态实例
            IFSMState<StateEnum, Parameter> state = FSMGetStateInstance(FSMNowState);

            if (state != null)
            {
                //是否离开状态成功
                //如果离开状态失败，不进入下一状态
                if (state.OnStateExit(nextState, FSMParameter) == false)
                {
                    return;
                }
            }

            //下一状态实例
            state = FSMGetStateInstance(nextState);
            if (state != null)
            {
                //是否进入状态成功
                if (state.OnStateEnter(FSMNowState, FSMParameter) == true)
                {
                    FSMNowState = nextState;
                }
            }

        }
    }
}
