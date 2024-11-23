namespace MungFramework.Algorithm
{
    public static class Json
    {
        public static string GetBoolJson(this bool value)
        {
            return value ? "true" : "false";
        }
        public static void FromBoolJsonOverwrite(this ref bool target, string json)
        {
            target = json == "true";
        }
        public static bool FromBoolJson(string json)
        {
            return json == "true";
        }
    }
}
