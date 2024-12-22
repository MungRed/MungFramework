using System.Collections;
using System.Text;

namespace MungFramework.Core
{
    /// <summary>
    /// 文件系统接口
    /// 通过实现该接口，可以实现不同平台的文件系统
    /// </summary>
    public interface IFileSystem
    {
        //统一使用UTF8编码
        public static readonly Encoding GlobalEncoding = Encoding.UTF8;
        public static string GetFullPath(string path, string filename, string format) => path + "/" + filename + "." + format;


        public bool HaveDirectory(string path);
        public bool HaveFile(string path, string filename, string format);
        public void DeleteDirectory(string directoryPath);
        public bool DeleteFile(string path, string filename, string format);


        public void WriteAllBytes(string path, string filename, string format, byte[] bytes);
        public IEnumerator WriteAllBytesAsync(string path, string filename, string format, byte[] bytes);

        public byte[] ReadAllBytes(string path, string filename, string format);
        public IEnumerator ReadAllBytesAsync(string path, string filename, string format, UnityEngine.Events.UnityAction<byte[]> resultAction);

        public string ReadText(string path, string filename, string format);
        public IEnumerator ReadTextAsync(string path, string filename, string format, UnityEngine.Events.UnityAction<string> resultAction);

        public void WriteText(string path, string filename, string format, string content);
        public IEnumerator WriteTextAsync(string path, string filename, string format, string content);


        #region 加密
        public enum LockOperate
        {
            Lock, UnLock
        }
        public static void LockFile(byte[] bytes, LockOperate op)
        {
            bool useLock = false;
            if (!useLock)
            {
                return;
            }
            byte index = 1;
            switch (op)
            {
                case LockOperate.Lock:
                    index = 1;
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        bytes[i] += index;
                        index++;
                        if (index == 5)
                        {
                            index = 0;
                        }
                    }
                    break;
                case LockOperate.UnLock:
                    index = 1;
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        bytes[i] -= index;
                        index++;
                        if (index == 5)
                        {
                            index = 0;
                        }
                    }
                    break;
            }
        }
        #endregion  
    }
}
