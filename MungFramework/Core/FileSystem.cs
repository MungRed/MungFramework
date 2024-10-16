using System.Collections;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MungFramework.Core
{
    public static class FileSystem
    {
        //ͳһʹ��UTF8����
        private static readonly Encoding GlobalEncoding = Encoding.UTF8;

        public static bool HaveDirectory(string path)
        {
            return Directory.Exists(path);
        }

        public static bool HaveFile(string path, string filename, string format)
        {
            string filepath = path + "/" + filename + "." + format;
            return File.Exists(filepath);
        }

        #region WriteBytes
        public static void WriteAllBytes(string path, string filename, string format, byte[] bytes)
        {
            string filePath = path + "/" + filename + "." + format;

            //Debug.Log("д���ļ�" + filePath);
            if (!Directory.Exists(path))
            {
                Debug.LogError("·�������ڣ�д���ļ�ʧ��" + filePath);
                return;
            }

            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                LockFile(bytes, LockOperate.Lock);
                fileStream.Write(bytes, 0, bytes.Length);
            }
        }
        public static IEnumerator WriteAllBytesAsync(string path, string filename, string format, byte[] bytes)
        {
            async Task writeAllBytesAsync(string filePath, byte[] bytes)
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    LockFile(bytes, LockOperate.Lock);
                    await fileStream.WriteAsync(bytes, 0, bytes.Length);
                }
            }

            string filePath = path + "/" + filename + "." + format;
            //Debug.Log("д���ļ�" + filePath);
            if (!Directory.Exists(path))
            {
                Debug.LogError("·�������ڣ�д���ļ�ʧ��" + filePath);
                yield break;
            }
            var task = writeAllBytesAsync(filePath, bytes);
            yield return new WaitUntil(() => task.IsCompleted);
            //Debug.Log("д���ļ��ɹ�" + filePath);
        }
        #endregion


        #region ReadBytes
        public static byte[] ReadAllBytes(string path, string filename, string format)
        {
            string filePath = path + "/" + filename + "." + format;
            //Debug.Log("��ȡ�ļ�" + filePath);

            if (!HaveDirectory(path) || !HaveFile(path, filename, format))
            {
                Debug.LogError("·�����ļ������ڣ���ȡ�ļ�ʧ��" + filePath);
                return null;
            }
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] bytes = new byte[fileStream.Length];
                fileStream.Read(bytes, 0, bytes.Length);
                LockFile(bytes, LockOperate.UnLock);
                return bytes;
            }
        }
        public static IEnumerator ReadAllBytesAsync(string path, string filename, string format, UnityEngine.Events.UnityAction<byte[]> resultAction)
        {
            async Task<byte[]> readAllBytesAsync(string filePath)
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    byte[] bytes = new byte[fileStream.Length];
                    await fileStream.ReadAsync(bytes, 0, bytes.Length);
                    LockFile(bytes, LockOperate.UnLock);
                    return bytes;
                }
            }

            string filePath = path + "/" + filename + "." + format;
            //Debug.Log("��ȡ�ļ�" + filePath);
            if (!HaveDirectory(path)|| !HaveFile(path, filename, format))
            {
                Debug.LogError("·�����ļ������ڣ���ȡ�ļ�ʧ��" + filePath);
                resultAction.Invoke(null);
                yield break;
            }
            var task = readAllBytesAsync(filePath);
            yield return new WaitUntil(() => task.IsCompleted);
            resultAction.Invoke(task.Result);
            //Debug.Log("��ȡ�ļ��ɹ�" + filePath);
        }
        #endregion

        #region ReadFile
        public static string ReadFile(string path, string filename, string format)
        {
            string filePath = path + "/" + filename + "." + format;
            //Debug.Log("��ȡ�ļ�" + filePath);
            var bytes = ReadAllBytes(path, filename, format);
            LockFile(bytes, LockOperate.UnLock);
            return GlobalEncoding.GetString(bytes);
        }
        public static IEnumerator ReadFileAsync(string path, string filename, string format,UnityEngine.Events.UnityAction<string> resultAction)
        {
            async Task<string> readFileAsync(string filePath)
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    byte[] bytes = new byte[fileStream.Length];
                    await fileStream.ReadAsync(bytes, 0, bytes.Length);
                    LockFile(bytes, LockOperate.UnLock);
                    return GlobalEncoding.GetString(bytes);
                }
            }

            string filePath = path + "/" + filename + "." + format;
            //Debug.Log("��ȡ�ļ�" + filePath);
            if (!HaveDirectory(path)|| !HaveFile(path, filename, format))
            {
                //Debug.LogError("·�����ļ������ڣ���ȡ�ļ�ʧ��" + filePath);
                resultAction.Invoke(null);
                yield break;
            }

            var task = readFileAsync(filePath);
            yield return new WaitUntil(() => task.IsCompleted);

            resultAction.Invoke(task.Result);
            //Debug.Log("��ȡ�ļ��ɹ�" + filePath);
        }
        #endregion

        #region WriteFile
        public static void WriteFile(string path, string filename, string format, string content)
        {
            string filePath = path + "/" + filename + "." + format;
            //Debug.Log("д���ļ�" + filePath);
            if (!Directory.Exists(path))
            {
                Debug.LogError("·�������ڣ�д���ļ�ʧ��" + filePath);
                return;
            }
            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                byte[] bytes = GlobalEncoding.GetBytes(content);
                LockFile(bytes, LockOperate.Lock);
                fileStream.Write(bytes, 0, bytes.Length);
            }
        }
        public static IEnumerator WriteFileAsync(string path, string filename, string format, string content)
        {
            async Task writeFileAsync(string filePath, string content)
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    byte[] bytes = GlobalEncoding.GetBytes(content);
                    LockFile(bytes, LockOperate.Lock);
                    await fileStream.WriteAsync(bytes, 0, bytes.Length);
                }
            }

            string filePath = path + "/" + filename + "." + format;
            //Debug.Log("д���ļ�" + filePath);
            if (!Directory.Exists(path))
            {
                Debug.LogError("·�������ڣ�д���ļ�ʧ��" + filePath);
                yield break;
            }

            var task = writeFileAsync(filePath, content);
            yield return new WaitUntil(() => task.IsCompleted);
            //Debug.Log("д���ļ��ɹ�" + filePath);
        }
        #endregion


        /// <summary>
        /// ɾ���ļ���
        /// </summary>
        public static  void DeleteDirectory(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                //ɾ��DatabasePath�µ������ļ�����ɾ���ļ���
                string[] files = Directory.GetFiles(directoryPath);
                foreach (string file in files)
                {
                    File.Delete(file);
                }
                Directory.Delete(directoryPath);
            }
        }
        /// <summary>
        /// ɾ���ļ�
        /// </summary>
        public static bool DeleteFile(string path, string filename, string format)
        {
            string filepath = path + "/" + filename + "." + format;

            if (!Directory.Exists(path)|| !File.Exists(filepath))
            {
                Debug.LogError("·�����ļ������ڣ�ɾ���ļ�ʧ��" + filepath);
                return false;
            }

            try
            {
                File.Delete(filepath);
                Debug.Log("ɾ���ļ��ɹ�" + filepath);
                return true;
            }
            catch
            {
                Debug.LogError("ɾ���ļ�ʧ��" + filepath);
                return false;
            }
        }


        private enum LockOperate
        {
            Lock, UnLock
        }
        private static void LockFile(byte[] bytes, LockOperate op)
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
    }
}

