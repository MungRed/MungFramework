using MungFramework.Core;
using MungFramework.Logic.MungBuffer;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace MungFramework.Logic.Save
{
    public abstract class SaveManagerAbstract : SingletonGameManagerAbstract<SaveManagerAbstract>
    {
        [SerializeField]
        [ReadOnly]
        protected List<SavableDataGameManagerAbstract> SavableManagerList;
        [SerializeField]
        protected SaveFile SystemSaveFile; //系统存档文件
        [SerializeField]
        protected SaveFile CurrentSaveFile;//当前存档文件
        [SerializeField]
        protected string GameSceneName; //游戏场景的名字，用于加载存档后进入游戏场景

        [SerializeField]
        protected MungBufferModel<string, Sprite> SaveImageBuffer = new(); //存档图片的缓存

        //存档数上限
        public static int SaveFileCount => 10;

        public override IEnumerator OnSceneLoad(GameManagerAbstract parentManager)
        {
            yield return base.OnSceneLoad(parentManager);
            //初始化数据库
            yield return InitDatabase();
            //加载存档文件,加载存档文件时默认数据库初始化成功，即存在系统存档文件
            yield return LoadSaves();
            //加载存档图片
            yield return LoadSaveImage();
        }

        /// <summary>
        /// 初始化数据库
        /// </summary>
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
        /// 加载存档图片
        /// </summary>
        protected virtual IEnumerator LoadSaveImage()
        {
            int nowCnt = 0;
            int totalCnt = 0;
            SaveImageBuffer.Clear();

            UnityAction<int,Sprite> action = (i,x) => {           
                nowCnt++;
                if (x != null)
                {
                    SaveImageBuffer.UpdateBuffer("save"+i, x);
                }
            };

            for (int i = 0; i <= SaveFileCount; i++)
            {
                totalCnt++;
                int t = i;
                StartCoroutine(ImageSystem.GetImageAsync("save" + i, x=>action(t,x)));
            }
            yield return new WaitUntil(()=>nowCnt >= totalCnt);
        }

        /// <summary>
        /// 加载系统存档文件
        /// </summary>
        protected virtual IEnumerator LoadSystemSaveFile()
        {
            SaveFile saveFile = null;
            yield return LoadSaveFile("system", x => saveFile = x);

            if (saveFile == null)
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

                SetSystemSaveValue("NowSaveFileName", nowSaveFileName.value);
            }


            SaveFile saveFile = null;
            yield return LoadSaveFile(nowSaveFileName.value, x => saveFile = x);
            Debug.Log("当前使用的存档为" + nowSaveFileName.value);

            //如果没有存档文件
            if (saveFile == null)
            {          
                Debug.Log("存档不存在，新建" + nowSaveFileName.value);
                //新建
                CurrentSaveFile = new SaveFile() { SaveName = nowSaveFileName.value,};
                //并保存
                yield return SaveInAsync(CurrentSaveFile);
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
        protected virtual void OnSave()
        {
            //通知所有SavableManager存档
            foreach (var savableManager in SavableManagerList)
            {
                savableManager.Save();
            }
        }

        /// <summary>
        /// 自动保存
        /// 保存至save0
        /// </summary>
        public virtual IEnumerator AutoSave()
        {
            //保存一张屏幕截图
            Texture2D screenShot = null;
            yield return ImageSystem.ScreenShot(x => screenShot = x);
            yield return SaveInByIndex(0,saveImage:screenShot);
        }

        /// <summary>
        /// 是否存在某个存档
        /// </summary>
        public virtual bool ExistSaveFile(string saveName)
        {
            return Database.ExistDataTable(saveName);
        }

        /// <summary>
        /// 载入某个存档同时进入游戏场景
        /// </summary>
        public virtual IEnumerator LoadSaveAndToGameScene(int saveIndex)
        {
            var saveName = "save" + saveIndex;
            if (!ExistSaveFile(saveName))
            {
                Debug.LogError("目标存档文件不存在，不能Reload存档:" + saveName);
                yield break;
            }
            yield return SetSystemSaveValueIEnumerator("NowSaveFileName", saveName);
            SceneManager.LoadScene(GameSceneName);
        }

        /// <summary>
        /// 载入某个存档同时不进入游戏场景，用于标题界面的异步加载场景
        /// </summary>
        public virtual IEnumerator LoadSaveButNotToGameScene(int saveIndex,UnityAction loadSuccess)
        {
            var saveName = "save" + saveIndex;
            if (!ExistSaveFile(saveName))
            {
                Debug.LogError("目标存档文件不存在，不能Reload存档:" + saveName);
                yield break;
            }
            yield return SetSystemSaveValueIEnumerator("NowSaveFileName", saveName);
            if (loadSuccess != null)
            {
                loadSuccess.Invoke();
            }
        }


        /// <summary>
        /// 把当前存档保存到指定存档
        /// </summary>
        public virtual IEnumerator SaveInByIndex(int saveIndex, bool onSave = true,Texture2D saveImage = null)
        {
            //是否在存档之前调用
            if (onSave)
            {
                OnSave();
            }

            CurrentSaveFile.SaveName = "save" + saveIndex;
            SetSystemSaveValue("NowSaveFileName", CurrentSaveFile.SaveName);

            //如果有存档截图，保存图标并更新缓存
            if (saveImage != null)
            {
                yield return ImageSystem.SaveImageAsync(CurrentSaveFile.SaveName, saveImage);
                Sprite sprite = ImageSystem.TextureToSprite(saveImage);
                SaveImageBuffer.UpdateBuffer(CurrentSaveFile.SaveName, sprite);
            }
            yield return SaveInAsync(CurrentSaveFile);
        }

        /// <summary>
        /// 储存一个空存档
        /// </summary>
        public virtual IEnumerator SaveEmptyInByIndex(int saveIndex)
        {
            SaveFile saveFile = new SaveFile() { 
                SaveName = "save" + saveIndex,
            };
            yield return SaveInAsync(saveFile);
        }

        /// <summary>
        /// 根据索引删除存档文件
        /// </summary>
        public void DeleteSaveFileByIndex(int saveIndex)
        {
            Database.RemoveDataTable("save" + saveIndex);
        }

        /// <summary>
        /// 保存存档文件
        /// </summary>
        protected virtual IEnumerator SaveInAsync(SaveFile saveFile)
        {
            yield return Database.SetKeyValuesAsync(saveFile.SaveName, saveFile.GetKeyValues());
        }
        protected virtual void SaveIn(SaveFile saveFile)
        {
            Database.SetKeyValues(saveFile.SaveName, saveFile.GetKeyValues());
        }


        /// <summary>
        /// 获取所有存档文件，0号存档为自动存档
        /// </summary>
        public virtual IEnumerator GetAllSaveFile(Dictionary<int,SaveFile> saveFileList)
        {
            saveFileList.Clear();;
            for (int i = 0; i <= SaveFileCount; i++)
            {
                int t = i;
                StartCoroutine(LoadSaveFile("save" + i, x => saveFileList.Add(t, x)));
            }
            yield return new WaitUntil(()=> saveFileList.Count >= SaveFileCount + 1);
        }

        /// <summary>
        /// 获得当前存档名
        /// </summary>
        public virtual string GetNowSaveName()
        {
            return CurrentSaveFile.SaveName;
        }

        /// <summary>
        /// 根据索引获取存档文件
        /// </summary>
        public virtual IEnumerator GetSaveFile(int saveIndex,UnityAction<SaveFile> resultAction)
        {
            yield return LoadSaveFile("save" + saveIndex, resultAction);
        }

        /// <summary>
        /// 加载存档文件
        /// </summary>
        protected virtual IEnumerator LoadSaveFile(string saveName, UnityAction<SaveFile> resultAction)
        {
            List<KeyValuePair<string, string>> saveData = null;
            string saveTime = "";
            yield return Database.GetKeyValuesAsync(saveName, (x, y) => { saveData = x; saveTime = y; });

            if (saveData != null)
            {
                SaveFile res = new SaveFile()
                {
                    SaveName = saveName,
                    SaveTime = saveTime,
                    SaveImage = SaveImageBuffer.GetBuffer(saveName).value
                };
                res.SetKeyValues(saveData);
                resultAction.Invoke(res);
            }
            else
            {
                resultAction.Invoke(null);
            }
        }

        /// <summary>
        /// 清空系统存档
        /// </summary>
        protected virtual void ClearSystemSave()
        {
            SystemSaveFile.SaveName = "system";
            SystemSaveFile.Clear();
            SaveIn(SystemSaveFile);
        }

        /// <summary>
        /// 设置系统存档值
        /// </summary>
        public virtual void SetSystemSaveValue(string key, string val)
        {
            SystemSaveFile.SetValue(key, val);
            //保存系统存档
            SaveIn(SystemSaveFile);
        }

        /// <summary>
        /// 异步设置系统存档值
        /// </summary>
        public virtual IEnumerator SetSystemSaveValueIEnumerator(string key, string val)
        {
            SystemSaveFile.SetValue(key, val);
            //保存系统存档
            yield return SaveInAsync(SystemSaveFile);
        }

        /// <summary>
        /// 获取系统存档值
        /// </summary>
        public virtual (string value, bool hasValue) GetSystemSaveValue(string key)
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
        public virtual (string value, bool hasValue) GetSaveValue(string key)
        {
            return CurrentSaveFile.GetValue(key);
        }


        public virtual void AddManager(SavableDataGameManagerAbstract savableManager)
        {
            if (!SavableManagerList.Contains(savableManager))
            {
                SavableManagerList.Add(savableManager);
            }
        }
    }
}
