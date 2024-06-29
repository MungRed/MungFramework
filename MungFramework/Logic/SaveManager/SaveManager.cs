using MungFramework.Core;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MungFramework.Logic.Save
{
    public abstract class SaveManager : GameManager
    {
        [SerializeField]
        [ReadOnly]
        protected List<GameSavableManager> SavableManagers;


        [SerializeField]
        //[ReadOnly]
        protected SaveFile SystemSaveFile; //系统存档文件

        [SerializeField]
        //[ReadOnly]
        protected SaveFile CurrentSaveFile;//当前存档文件


        public override IEnumerator OnSceneLoad(GameManager parentManager)
        {
            yield return StartCoroutine(base.OnSceneLoad(parentManager));

            //初始化数据库
            //如果初始化数据库失败抛出异常提示
            yield return StartCoroutine(InitDataBase());

            //加载存档文件
            //加载存档文件时默认数据库初始化成功，即存在系统存档文件
            yield return StartCoroutine(LoadSaves());
        }

        /// <summary>
        /// 初始化数据库
        /// </summary>
        /// <exception cref="System.Exception"></exception>
        protected IEnumerator InitDataBase()
        {
            bool hasDataBase = DataBase.ExistDataBase();
            if (!hasDataBase)
            {
                Debug.Log("数据库不存在，新建");
                bool createSuccess = DataBase.CreateDataBase();
                if (!createSuccess)
                {
                    //如果创建失败
                    //抛出异常
                    throw new System.Exception("数据库创建失败");
                }
            }
            yield return null;
        }

        /// <summary>
        /// 加载存档文件
        /// 加载存档文件后，系统文件和当前存档文件都不为null
        /// </summary>
        protected IEnumerator LoadSaves()
        {
            yield return StartCoroutine(LoadSystemSaveFile());
            yield return StartCoroutine(LoadPlayerSaveFiles());
        }

        /// <summary>
        /// 加载系统存档文件
        /// </summary>
        protected IEnumerator LoadSystemSaveFile()
        {
            var loadResult = LoadSaveFile("system");

            if (loadResult.Item2 == false)
            {
                throw new System.Exception("系统存档文件加载失败");
            }
            else
            {
                SystemSaveFile = loadResult.Item1;
            }
            yield return null;
        }

        /// <summary>
        /// 加载玩家存档文件
        /// </summary>
        protected IEnumerator LoadPlayerSaveFiles()
        {
            //获取当前使用的存档
            var nowSaveFileName = SystemSaveFile.GetValue("NowSaveFileName");

            //如果没有当前使用的存档，说明是第一次进入游戏，默认使用自动存档
            if (nowSaveFileName.hasVal == false)
            {
                nowSaveFileName.val = "save0";
                SetSystemValue("NowSaveFileName", "save0");
            }

            var loadResult = LoadSaveFile(nowSaveFileName.val);
            if (loadResult.Item2 == false)
            {
                CurrentSaveFile = new SaveFile(nowSaveFileName.val, new());
            }
            else
            {
                CurrentSaveFile = loadResult.Item1;
            }

            yield return null;
        }

        /// <summary>
        /// 在存档之前调用
        /// </summary>
        protected IEnumerator OnSave()
        {
            //保存所有管理器
            foreach (var savableManager in SavableManagers)
            {
                yield return StartCoroutine (savableManager.Save());
            }
        }

        /// <summary>
        /// 自动保存
        /// </summary>
        public IEnumerator AutoSave()
        {
            yield return StartCoroutine(OnSave());

            SaveFile saveFile = new SaveFile("save0", CurrentSaveFile.GetKeyValues());

            yield return StartCoroutine(SaveIn(saveFile));
        }

        /// <summary>
        /// 把当前存档保存到指定存档
        /// </summary>
        /// <param name="saveIndex"></param>
        public IEnumerator SaveInByIndex(int saveIndex)
        {
            yield return StartCoroutine(OnSave());

            CurrentSaveFile.SaveName = "save" + saveIndex;

            yield return StartCoroutine(SaveIn(CurrentSaveFile));

            //更新当前存档
            SetSystemValue("NowSaveFileName", CurrentSaveFile.SaveName);
        }

        protected IEnumerator SaveIn(SaveFile saveFile)
        {
            DataBase.SetKeyValues(saveFile.SaveName, saveFile.GetKeyValues());
            yield return null;
        }



        public (SaveFile, bool) LoadSaveFile(int saveindex)
        {
            string saveName = "save" + saveindex;
            return LoadSaveFile(saveName);
        }

        public (SaveFile, bool) LoadSaveFile(string saveName)
        {
            var saveFile = DataBase.GetKeyValues(saveName);
            if (saveFile.Item2 == false)
            {
                return (new SaveFile(saveName, new()), false);
            }
            return (new SaveFile(saveName, saveFile.Item1), true);
        }





        /// <summary>
        /// 设置系统存档值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void SetSystemValue(string key, string val)
        {
            SystemSaveFile.SetValue(key, val);
            SaveIn(SystemSaveFile);
        }

        /// <summary>
        /// 获取系统存档值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public (string, bool) GetSystemValue(string key)
        {
            return SystemSaveFile.GetValue(key);
        }

        /// <summary>
        /// 设置当前存档值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void SetSaveValue(string key, string val)
        {
            CurrentSaveFile.SetValue(key, val);
        }

        /// <summary>
        /// 获取当前存档值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public (string, bool) GetSaveValue(string key)
        {
            return CurrentSaveFile.GetValue(key);
        }


        public void AddManager(GameSavableManager savableManager)
        {
            if (SavableManagers.Contains(savableManager))
            {
                return;
            }
            SavableManagers.Add(savableManager);
        }

    }
}
