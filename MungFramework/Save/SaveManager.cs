using MungFramework.Logic;
using System.Collections.Generic;

namespace MungFramework.Save
{
    public abstract class SaveManager : GameManager
    {
        public List<GameSaveableManager> SaveableManagers;


        public void AddManager(GameSaveableManager saveableManager)
        {
            if(SaveableManagers.Contains(saveableManager))
            {
                return;
            }
            SaveableManagers.Add(saveableManager);
        }

    }
}
