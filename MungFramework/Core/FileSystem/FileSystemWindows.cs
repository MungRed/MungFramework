using System.Collections;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using static MungFramework.Core.IFileSystem;

namespace MungFramework.Core
{
    public class FileSystemWindows : IFileSystem
    {
        public bool HaveDirectory(string path)
        {
            return Directory.Exists(path);
        }

        public bool HaveFile(string path, string filename, string format)
        {
            string filepath = GetFullPath(path, filename, format);
            return File.Exists(filepath);
        }

        public void DeleteDirectory(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                //删除DatabasePath下的所有文件，再删除文件夹
                string[] files = Directory.GetFiles(directoryPath);
                foreach (string file in files)
                {
                    File.Delete(file);
                }
                Directory.Delete(directoryPath);
            }
        }

        public bool DeleteFile(string path, string filename, string format)
        {
            string filepath = GetFullPath(path, filename, format);

            if (!Directory.Exists(path) || !File.Exists(filepath))
            {
                Debug.LogError("路径或文件不存在，删除文件失败" + filepath);
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
                Debug.LogError("删除文件失败" + filepath);
                return false;
            }
        }

        public void WriteAllBytes(string path, string filename, string format, byte[] bytes)
        {
            string filePath = GetFullPath(path, filename, format);

            //Debug.Log("写入文件" + filePath);
            if (!HaveDirectory(path))
            {
                Debug.LogError("路径不存在，写入文件失败" + filePath);
                return;
            }

            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                LockFile(bytes, LockOperate.Lock);
                fileStream.Write(bytes, 0, bytes.Length);
            }
        }

        public IEnumerator WriteAllBytesAsync(string path, string filename, string format, byte[] bytes)
        {
            async Task writeAllBytesAsync(string filePath, byte[] bytes)
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    LockFile(bytes, LockOperate.Lock);
                    await fileStream.WriteAsync(bytes, 0, bytes.Length);
                }
            }

            string filePath = GetFullPath(path, filename, format);
            //Debug.Log("写入文件" + filePath);
            if (!HaveDirectory(path))
            {
                Debug.LogError("路径不存在，写入文件失败" + filePath);
                yield break;
            }
            var task = writeAllBytesAsync(filePath, bytes);
            yield return new WaitUntil(() => task.IsCompleted);
            //Debug.Log("写入文件成功" + filePath);
        }

        public byte[] ReadAllBytes(string path, string filename, string format)
        {
            string filePath = GetFullPath(path, filename, format);
            //Debug.Log("读取文件" + filePath);

            if (!HaveDirectory(path) || !HaveFile(path, filename, format))
            {
                Debug.LogError("路径或文件不存在，读取文件失败" + filePath);
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

        public IEnumerator ReadAllBytesAsync(string path, string filename, string format, UnityAction<byte[]> resultAction)
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

            string filePath = GetFullPath(path, filename, format);
            //Debug.Log("读取文件" + filePath);
            if (!HaveDirectory(path) || !HaveFile(path, filename, format))
            {
                Debug.LogError("路径或文件不存在，读取文件失败" + filePath);
                resultAction.Invoke(null);
                yield break;
            }
            var task = readAllBytesAsync(filePath);
            yield return new WaitUntil(() => task.IsCompleted);
            resultAction.Invoke(task.Result);
            //Debug.Log("读取文件成功" + filePath);
        }

        public string ReadText(string path, string filename, string format)
        {
            string filePath = GetFullPath(path, filename, format);
            //Debug.Log("读取文件" + filePath);
            var bytes = ReadAllBytes(path, filename, format);
            LockFile(bytes, LockOperate.UnLock);
            return GlobalEncoding.GetString(bytes);
        }

        public IEnumerator ReadTextAsync(string path, string filename, string format, UnityAction<string> resultAction)
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

            string filePath = GetFullPath(path, filename, format);
            //Debug.Log("读取文件" + filePath);
            if (!HaveDirectory(path) || !HaveFile(path, filename, format))
            {
                //Debug.LogError("路径或文件不存在，读取文件失败" + filePath);
                resultAction.Invoke(null);
                yield break;
            }

            var task = readFileAsync(filePath);
            yield return new WaitUntil(() => task.IsCompleted);

            resultAction.Invoke(task.Result);
            //Debug.Log("读取文件成功" + filePath);
        }



        public void WriteText(string path, string filename, string format, string content)
        {
            string filePath = GetFullPath(path, filename, format);
            //Debug.Log("写入文件" + filePath);
            if (!HaveDirectory(path))
            {
                Debug.LogError("路径不存在，写入文件失败" + filePath);
                return;
            }
            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                byte[] bytes = GlobalEncoding.GetBytes(content);
                LockFile(bytes, LockOperate.Lock);
                fileStream.Write(bytes, 0, bytes.Length);
            }
        }

        public IEnumerator WriteTextAsync(string path, string filename, string format, string content)
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

            string filePath = GetFullPath(path, filename, format);
            //Debug.Log("写入文件" + filePath);
            if (!HaveDirectory(path))
            {
                Debug.LogError("路径不存在，写入文件失败" + filePath);
                yield break;
            }

            var task = writeFileAsync(filePath, content);
            yield return new WaitUntil(() => task.IsCompleted);
            //Debug.Log("写入文件成功" + filePath);
        }
    }
}
