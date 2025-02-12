﻿using MungFramework.Logic.Input;
using System;

namespace MungFramework.Logic.FSM
{
    /// <summary>
    /// 有限状态机管理器接口
    /// </summary>
    public interface IFSMManager<T_StateEnum, T_Parameter> where T_StateEnum : Enum where T_Parameter : IFSMParameter
    {
        public IFSMManager<T_StateEnum, T_Parameter> FSManager
        {
            get;
        }
        public T_Parameter FSMParameter
        {
            get;
        }

        /// <summary>
        /// 当前状态
        /// </summary>
        public T_StateEnum FSMNowState
        {
            set; get;
        }

        /// <summary>
        /// 获取每个状态对应的实例
        /// 约束：每个状态只能有一个实例
        /// </summary>
        public IFSMState<T_StateEnum, T_Parameter> FSMGetStateInstance(T_StateEnum state);

        /// <summary>
        /// 帧更新
        /// </summary>
        public void FSMUpdate()
        {
            //获取状态实例
            IFSMState<T_StateEnum, T_Parameter> state = FSMGetStateInstance(FSMNowState);
            //如果状态实例为空，返回
            if (state == null)
            {
                return;
            }

            //状态帧更新，并获取下一状态
            T_StateEnum nextState = state.OnStateUpdate(FSMParameter);
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
            IFSMState<T_StateEnum, T_Parameter> state = FSMGetStateInstance(FSMNowState);
            //如果状态实例为空，返回
            if (state == null)
            {
                return;
            }

            //状态固定帧更新，并获取下一状态
            T_StateEnum nextState = state.OnStateFixedUpdate(FSMParameter);
            //如果下一状态不等于当前状态，改变状态
            if (!nextState.Equals(FSMNowState))
            {
                FSMChangeState(nextState);
            }
        }
        public void FSMLateUpdate()
        {
            //获取状态实例
            IFSMState<T_StateEnum, T_Parameter> state = FSMGetStateInstance(FSMNowState);
            //如果状态实例为空，返回
            if (state == null)
            {
                return;
            }

            //延迟帧更新，并获取下一状态
            T_StateEnum nextState = state.OnStateLateUpdate(FSMParameter);
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
            IFSMState<T_StateEnum, T_Parameter> state = FSMGetStateInstance(FSMNowState);
            //如果状态实例为空，返回
            if (state == null)
            {
                return;
            }

            //状态按键输入，并获取下一状态
            T_StateEnum nextState = state.OnStateInput(inputValue, FSMParameter);
            //如果下一状态不等于当前状态，改变状态
            if (!nextState.Equals(FSMNowState))
            {
                FSMChangeState(nextState);
            }
        }

        /// <summary>
        /// 改变状态，只有当前状态和下一状态都正常才会切换
        /// 即不允许从null切换到非null或从非null切换到null
        /// </summary>
        public void FSMChangeState(T_StateEnum nextState)
        {
            //当前状态实例
            IFSMState<T_StateEnum, T_Parameter> nowStateInstance = FSMGetStateInstance(FSMNowState);
            //下一状态实例
            IFSMState<T_StateEnum, T_Parameter> nextStateInstance = FSMGetStateInstance(nextState);

            //只有当前状态不为空才能切换
            if (nowStateInstance != null)
            {
                //是否离开状态成功
                //如果离开状态失败，不进入下一状态
                if (nowStateInstance.OnStateExit(nextState, FSMParameter) == false)
                {
                    return;
                }
                //只有下一状态不为空才能切换
                if (nextStateInstance != null)
                {
                    //只有进入下一状态成功才会切换
                    if (nextStateInstance.OnStateEnter(FSMNowState, FSMParameter) == true)
                    {
                        FSMNowState = nextState;
                    }
                }
            }
        }

        /// <summary>
        /// 强制切换状态，不考虑当前状态是否正常
        /// 即可以切换至null或从null切换，且不考虑状态切换成功与否
        /// </summary>
        public void FSMForceChangeState(T_StateEnum nextState)
        {            
            //当前状态实例
            IFSMState<T_StateEnum, T_Parameter> nowStateInstance = FSMGetStateInstance(FSMNowState);
            //下一状态实例
            IFSMState<T_StateEnum, T_Parameter> nextStateInstance = FSMGetStateInstance(nextState);

            if (nowStateInstance != null)
            {
                nowStateInstance.OnStateExit(nextState, FSMParameter);
            }
            if (nextStateInstance != null)
            {
                nextStateInstance.OnStateEnter(FSMNowState, FSMParameter);
            }
            FSMNowState = nextState;
        }
    }
}
