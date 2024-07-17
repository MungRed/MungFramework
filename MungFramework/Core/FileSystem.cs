using System.Collections;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MungFramework.Core
{
    public static class FileSystem
    {
        //统一使用UTF8编码
        private static readonly Encoding GlobalEncoding = Encoding.UTF8;

        public static bool HasDirectory(string path)
        {
            return Directory.Exists(path);
        }

        public static bool HasFile(string path, string filename, string format)
        {
            string filepath = path + "/" + filename + "." + format;
            return File.Exists(filepath);
        }

        public static IEnumerator ReadFileAsync(string path, string filename, string format,UnityEngine.Events.UnityAction<string> resultAction)
        {
            string filepath = path + "/" + filename + "." + format;
            Debug.Log("读取文件" + filepath);
            if (!HasDirectory(path))
            {
                Debug.Log("路径不存在，读取文件失败" + filepath);
                yield break;
            }
            if (!HasFile(path, filename, format))
            {
                Debug.Log("文件不存在，读取文件失败" + filepath);
                yield break;
            }

            var task = readFileAsync(filepath);
            yield return new WaitUntil(() => task.IsCompleted);

            resultAction.Invoke(task.Result);
            Debug.Log("读取文件成功" + filepath);
        }
        private static async Task<string> readFileAsync(string filePath)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] bytes = new byte[fileStream.Length];
                await fileStream.ReadAsync(bytes, 0, bytes.Length);
                LockFile(bytes, LockOperate.UnLock);
                return GlobalEncoding.GetString(bytes);
            }
        }
        public static IEnumerator WriteFileAsync(string path, string filename, string format, string content)
        {
            string filepath = path + "/" + filename + "." + format;
            Debug.Log("写入文件" + filepath);
            if (!Directory.Exists(path))
            {
                Debug.Log("路径不存在，写入文件失败" + filepath);
                yield break;
            }

            var task = writeFileAsync(filepath, content);
            yield return new WaitUntil(() => task.IsCompleted);
            Debug.Log("写入文件成功" + filepath);
        }
        private static async Task writeFileAsync(string filePath, string content)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                byte[] bytes = GlobalEncoding.GetBytes(content);
                LockFile(bytes, LockOperate.Lock);
                await fileStream.WriteAsync(bytes, 0, bytes.Length);
            }
        }

/*        /// <summary>
        /// 将文件读取为字符串
        /// </summary>
        /// <returns>Content,Success</returns>
        public static (string, bool) ReadFile(string path, string filename, string format)
        {
            string filepath = path + "/" + filename + "." + format;
            Debug.Log("读取文件" + filepath);

            if (!HasDirectory(path))
            {
                Debug.Log("路径不存在，读取文件失败" + filepath);
                return ("", false);
            }
            if (!HasFile(path,filename,format))
            {
                Debug.Log("文件不存在，读取文件失败" + filepath);
                return ("", false);
            }

            try
            {
                using (var fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read))
                {
                    byte[] bytes = new byte[fileStream.Length];
                    fileStream.Read(bytes, 0, bytes.Length);
                    LockFile(bytes, LockOperate.UnLock);
                    string content = GlobalEncoding.GetString(bytes);
                    Debug.Log("读取文件成功" + filepath);
                    return (content, true);
                }
            }
            catch
            {
                Debug.Log("读取文件失败" + filepath);
                return ("", false);
            }
        }

        /// <summary>
        /// 将字符串写入文件，如果文件不存在会创建文件
        /// </summary>
        /// <returns>Success</returns>
        public static bool WriteFile(string path, string filename, string format, string content)
        {
            string filepath = path + "/" + filename + "." + format;
            Debug.Log("写入文件" + filepath);

            if (!Directory.Exists(path))
            {
                Debug.Log("路径不存在，写入文件失败" + filepath);
                return false;
            }


            try
            {
                using (var fileStream = new FileStream(filepath, FileMode.Create, FileAccess.Write))
                {
                    byte[] bytes = GlobalEncoding.GetBytes(content);
                    LockFile(bytes, LockOperate.Lock);
                    fileStream.Write(bytes, 0, bytes.Length);
                    Debug.Log("写入文件成功" + filepath);
                    return true;
                }
            }
            catch
            {
                Debug.Log("写入文件失败" + filepath);
                return false;
            }
        }*/

        /// <summary>
        /// 删除文件
        /// </summary>
        public static bool DeleteFile(string path, string filename, string format)
        {

            string filepath = path + "/" + filename + "." + format;

            Debug.Log("删除文件" + filepath);
            if (!Directory.Exists(path))
            {
                Debug.Log("路径不存在，删除文件失败" + filepath);
                return false;
            }

            if (!File.Exists(filepath))
            {
                Debug.Log("文件不存在，删除文件失败" + filepath);
                return false;
            }

            try
            {
                File.Delete(filepath);
                Debug.Log("删除文件成功" + filepath);
                return true;
            }
            catch
            {
                Debug.Log("删除文件失败" + filepath);
                return false;
            }
        }

        private enum LockOperate
        {
            Lock, UnLock
        }
        private static void LockFile(byte[] bytes, LockOperate op)
        {
            /*byte index = 1;
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
            }*/
        }
    }
}

