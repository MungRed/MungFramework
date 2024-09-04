using MungFramework.Model;

namespace MungFramework.Logic.Input
{
    public class InputMapLayerSOModelStream:SOModelStream<InputMapLayerDataSO,InputMapLayer>
    {
        public InputMapLayer Stream(InputMapLayerDataSO so)
        {
            InputMapLayer res = new()
            {
                InputMapLayerName=so.InputMapLayerName
            };
            foreach (var inputItem in so.InputMapList)
            {
                res.AddBind(inputItem.InputKey, inputItem.InputValue);
            }
            return res;
        }

    }
}