using MungFramework.Core;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
                yield return Database.CreateDatabase();
            }
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
            SaveFile saveFile = null;
            yield return LoadSaveFile("system",x=>saveFile = x);

            if (saveFile==null)
            {
                //TODO : 如果有存档备份，可以尝试加载备份

                Debug.LogError("系统存档文件加载失败,重置存档");

                yield return Database.CreateDatabase();
                yield return LoadSystemSaveFile();
            }
            else
            {
                Debug.Log("系统存档文件加载成功");
                SystemSaveFile = saveFile;
            }
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

                SetSystemValue("NowSaveFileName", nowSaveFileName.value);
            }

            SaveFile saveFile = null;

            yield return LoadSaveFile(nowSaveFileName.value,x=>saveFile = x);
            //var loadResult = LoadSaveFile(nowSaveFileName.value);

            Debug.Log("当前使用的存档为" + nowSaveFileName.value);
            //如果没有存档文件
            if (saveFile == null)
            {
                Debug.Log("存档不存在，新建" + nowSaveFileName.value);
                //新建
                CurrentSaveFile = new SaveFile();
                CurrentSaveFile.SaveName = nowSaveFileName.value;
                //并保存
                yield return SaveIn(CurrentSaveFile);
            }
            else
             {
                //否则当前存档文件为读取的存档文件
                CurrentSaveFile = saveFile;
            }
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

            SaveFile saveFile = new SaveFile();
            saveFile.SaveName = "save0";
            saveFile.SetKeyValues(CurrentSaveFile.GetKeyValues());

            yield return SaveIn(saveFile);
        }


        /// <summary>
        /// 把当前存档保存到指定存档
        /// </summary>
        public virtual IEnumerator SaveInByIndex(int saveIndex)
        {
            yield return OnSave();

            CurrentSaveFile.SaveName = "save" + saveIndex;

            yield return SaveIn(CurrentSaveFile);

            //更新当前存档
            SetSystemValue("NowSaveFileName", CurrentSaveFile.SaveName);
        }

        /// <summary>
        /// 保存存档文件
        /// </summary>
        protected virtual IEnumerator SaveIn(SaveFile saveFile)
        {
            yield return Database.SetKeyValues(saveFile.SaveName, saveFile.GetKeyValues());
        }



        /// <summary>
        /// 通过存档索引加载存档文件
        /// </summary>
        public virtual  IEnumerator LoadSaveFile(int saveindex, UnityAction<SaveFile> resultAction)
        {
            string saveName = "save" + saveindex;
            yield return LoadSaveFile(saveName,resultAction);
        }


        /// <summary>
        /// 加载存档文件
        /// </summary>
        public virtual IEnumerator LoadSaveFile(string saveName,UnityAction<SaveFile> resultAction)
        {
            List<KeyValuePair<string, string>> saveData = null;
            yield return Database.GetKeyValues(saveName,x=>saveData = x);

            if (saveData != null)
            {
                SaveFile res = new SaveFile();
                res.SaveName = saveName;
                res.SetKeyValues(saveData);
                resultAction.Invoke(res);
            }
        }

        /// <summary>
        /// 设置系统存档值
        /// </summary>
        public virtual void SetSystemValue(string key, string val)
        {
            SystemSaveFile.SetValue(key, val);
            //保存系统存档
            StartCoroutine(SaveIn(SystemSaveFile));
        }

        /// <summary>
        /// 获取系统存档值
        /// </summary>
        public  virtual (string value , bool hasValue) GetSystemValue(string key)
        {
            return SystemSaveFile.GetValue(key);
        }

        /// <summary>
        /// 设置当前存档值
        /// </summary>
        public virtual void SetSaveValue(string key, string val)
        {
            CurrentSaveFile.SetValue(key, val);
        }

        /// <summary>
        /// 获取当前存档值
        /// </summary>
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
