using System;

namespace MungFramework.Logic.Input
{
    [Serializable]
    public class InputMapKeyValuePair
    {
        //代表某个按键会触发某个值
        public InputKeyEnum InputKey;
        public InputValueEnum InputValue;

        public InputMapKeyValuePair(InputKeyEnum inputKey, InputValueEnum inputValue)
        {
            InputKey = inputKey;
            InputValue = inputValue;
        }
    }
}