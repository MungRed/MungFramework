using MungFramework.Core;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MungFramework.Logic.Save
{
    public abstract class SaveManagerAbstract : SingletonGameManagerAbstract<SaveManagerAbstract>
    {
        [SerializeField]
        [ReadOnly]
        protected List<SavableGameManagerAbstract> SavableManagerList;


        [SerializeField]
        protected SaveFile SystemSaveFile; //系统存档文件

        [SerializeField]
        protected SaveFile CurrentSaveFile;//当前存档文件


        public override IEnumerator OnSceneLoad(GameManagerAbstract parentManager)
        {
            yield return base.OnSceneLoad(parentManager);

            //初始化数据库
            //如果初始化数据库失败抛出异常提示
            yield return InitDatabase();

            //加载存档文件
            //加载存档文件时默认数据库初始化成功，即存在系统存档文件
            yield return LoadSaves();
        }


        /// <summary>
        /// 初始化数据库
        /// </summary>
        /// <exception cref="System.Exception"></exception>
        protected virtual IEnumerator InitDatabase()
        {
            bool hasDataBase = Database.ExistDatabase();

            //如果数据库不存在
            if (!hasDataBase)
            {
                Debug.Log("数据库不存在，新建");

                //创建数据库
                bool createSuccess = Database.CreateDatabase();
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
        protected virtual IEnumerator LoadSaves()
        {
            yield return LoadSystemSaveFile();

            yield return LoadPlayerSaveFiles();
        }

        /// <summary>
        /// 加载系统存档文件
        /// </summary>
        protected virtual IEnumerator LoadSystemSaveFile()
        {
            var loadResult = LoadSaveFile("system");

            if (loadResult.Item2 == false)
            {
                //TODO : 如果有存档备份，可以尝试加载备份

                Debug.LogError("系统存档文件加载失败,重置存档");

                Database.CreateDatabase();
                yield return LoadSystemSaveFile();
            }
            else
            {
                Debug.Log("系统存档文件加载成功");
                SystemSaveFile = loadResult.Item1;
            }
            yield return null;
        }

        /// <summary>
        /// 加载玩家存档文件
        /// </summary>
        protected virtual IEnumerator LoadPlayerSaveFiles()
        {
            //获取当前使用的存档
            var nowSaveFileName = SystemSaveFile.GetValue("NowSaveFileName");

            //如果没有当前使用的存档，说明是第一次进入游戏，默认使用自动存档
            if (nowSaveFileName.hasValue == false)
            {
                Debug.Log("没有当前使用的存档，说明是第一次进入游戏，默认使用自动存档");
                nowSaveFileName.value = "save0";

                SetSystemValue("NowSaveFileName", "save0");
            }

            var loadResult = LoadSaveFile(nowSaveFileName.value);
            //如果没有存档文件
            if (loadResult.Item2 == false)
            {
                //新建
                CurrentSaveFile = new SaveFile(nowSaveFileName.value, new());
                //并保存
                yield return SaveIn(CurrentSaveFile);
            }
            else
            {
                //否则当前存档文件为读取的存档文件
                CurrentSaveFile = loadResult.Item1;
            }

            yield return null;
        }

        /// <summary>
        /// 在存档之前调用
        /// </summary>
        protected virtual IEnumerator OnSave()
        {
            //保存所有管理器
            foreach (var savableManager in SavableManagerList)
            {
                yield return savableManager.Save();
            }
        }

        /// <summary>
        /// 自动保存
        /// </summary>
        public virtual IEnumerator AutoSave()
        {
            yield return OnSave();

            SaveFile saveFile = new SaveFile("save0", CurrentSaveFile.GetKeyValues());

            yield return SaveIn(saveFile);
        }

        /// <summary>
        /// 把当前存档保存到指定存档
        /// </summary>
        /// <param name="saveIndex"></param>
        public virtual IEnumerator SaveInByIndex(int saveIndex)
        {
            yield return OnSave();

            CurrentSaveFile.SaveName = "save" + saveIndex;

            yield return SaveIn(CurrentSaveFile);

            //更新当前存档
            SetSystemValue("NowSaveFileName", CurrentSaveFile.SaveName);
        }

        protected virtual IEnumerator SaveIn(SaveFile saveFile)
        {
            Database.SetKeyValues(saveFile.SaveName, saveFile.GetKeyValues());
            yield return null;
        }



        public virtual  (SaveFile saveFile, bool hasSaveFile) LoadSaveFile(int saveindex)
        {
            string saveName = "save" + saveindex;
            return LoadSaveFile(saveName);
        }

        public virtual (SaveFile saveFile, bool hasSaveFile) LoadSaveFile(string saveName)
        {
            var saveFile = Database.GetKeyValues(saveName);
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
        public virtual void SetSystemValue(string key, string val)
        {
            SystemSaveFile.SetValue(key, val);
            StartCoroutine(SaveIn(SystemSaveFile));
        }

        /// <summary>
        /// 获取系统存档值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public  virtual (string value , bool hasValue) GetSystemValue(string key)
        {
            return SystemSaveFile.GetValue(key);
        }

        /// <summary>
        /// 设置当前存档值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public virtual void SetSaveValue(string key, string val)
        {
            CurrentSaveFile.SetValue(key, val);
        }

        /// <summary>
        /// 获取当前存档值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual(string value, bool hasValue) GetSaveValue(string key)
        {
            return CurrentSaveFile.GetValue(key);
        }


        public virtual void AddManager(SavableGameManagerAbstract savableManager)
        {
            if (SavableManagerList.Contains(savableManager))
            {
                return;
            }
            SavableManagerList.Add(savableManager);
        }

    }
}
