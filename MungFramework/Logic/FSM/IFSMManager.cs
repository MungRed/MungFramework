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
            //获取状态实例
            IFSMState<StateEnum, Parameter> state = FSMGetStateInstance(FSMNowState);
            //如果状态实例为空，返回
            if (state == null)
            {
                return;
            }

            //状态帧更新，并获取下一状态
            StateEnum nextState = state.OnStateUpdate(FSMParameter);
            //如果下一状态不等于当前状态，改变状态
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
            //获取状态实例
            IFSMState<StateEnum, Parameter> state = FSMGetStateInstance(FSMNowState);
            //如果状态实例为空，返回
            if (state == null)
            {
                return;
            }

            //状态固定帧更新，并获取下一状态
            StateEnum nextState = state.OnStateFixedUpdate(FSMParameter);
            //如果下一状态不等于当前状态，改变状态
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
            //获取状态实例
            IFSMState<StateEnum, Parameter> state = FSMGetStateInstance(FSMNowState);
            //如果状态实例为空，返回
            if (state == null)
            {
                return;
            }

            //状态按键输入，并获取下一状态
            StateEnum nextState = state.OnStateInput(inputValue,FSMParameter);
            //如果下一状态不等于当前状态，改变状态
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
            IFSMState<StateEnum, Parameter> nowStateInstance = FSMGetStateInstance(FSMNowState);
            //下一状态实例
            IFSMState<StateEnum, Parameter> nextStateInstance = FSMGetStateInstance(nextState);
            if (nowStateInstance != null)
            {
                //是否离开状态成功
                //如果离开状态失败，不进入下一状态
                if (nowStateInstance.OnStateExit(nextState, FSMParameter) == false)
                {
                    return;
                }
            }
            if(nextStateInstance != null)
            {
                //是否进入状态成功
                if (nextStateInstance.OnStateEnter(FSMNowState, FSMParameter) == true)
                {
                    FSMNowState = nextState;
                }
            }
        }
    }
}
