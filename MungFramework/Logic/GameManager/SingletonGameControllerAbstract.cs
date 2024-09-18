namespace MungFramework.Logic
{
    public class SingletonGameControllerAbstract <T> : GameControllerAbstract where T: GameControllerAbstract
    {
        private static T _Instance;
        public static T Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = FindObjectOfType(typeof(T)) as T;
                }
                return _Instance;
            }
        }
    }
}
