namespace MungFramework.Model.MungBag.FlagBag
{
    public interface IFlagBag
    {
        public abstract void AddFlag(string flagName, int flagValue);
        public abstract FlagBagItem GetFlag(string flagName);
        public abstract bool HaveFlag(string flagName);
        public abstract int GetFlagValue(string flagName);
        public abstract bool RemoveFlag(string flagName);
        public abstract void ChangeFlagValue(string flagName, int flagValue);
        public abstract void DeltaFlagValue(string flagName, int deltaValue);
    }
}
