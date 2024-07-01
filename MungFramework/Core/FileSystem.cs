using System.IO;
using System.Text;
using UnityEngine;

namespace MungFramework.Core
{
    public static class FileSystem
    {
        //ͳһʹ��UTF8����
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


        /// <summary>
        /// ���ļ���ȡΪ�ַ���
        /// </summary>
        /// <returns>Content,Success</returns>
        public static (string, bool) ReadFile(string path, string filename, string format)
        {
            string filepath = path + "/" + filename + "." + format;
            Debug.Log("��ȡ�ļ�" + filepath);

            if (!HasDirectory(path))
            {
                Debug.Log("·�������ڣ���ȡ�ļ�ʧ��" + filepath);
                return ("", false);
            }
            if (!HasFile(path,filename,format))
            {
                Debug.Log("�ļ������ڣ���ȡ�ļ�ʧ��" + filepath);
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
                    Debug.Log("��ȡ�ļ��ɹ�" + filepath);
                    return (content, true);
                }
            }
            catch
            {
                Debug.Log("��ȡ�ļ�ʧ��" + filepath);
                return ("", false);
            }
        }

        /// <summary>
        /// ���ַ���д���ļ�������ļ������ڻᴴ���ļ�
        /// </summary>
        /// <returns>Success</returns>
        public static bool WriteFile(string path, string filename, string format, string content)
        {
            string filepath = path + "/" + filename + "." + format;
            Debug.Log("д���ļ�" + filepath);

            if (!Directory.Exists(path))
            {
                Debug.Log("·�������ڣ�д���ļ�ʧ��" + filepath);
                return false;
            }


            try
            {
                using (var fileStream = new FileStream(filepath, FileMode.Create, FileAccess.Write))
                {
                    byte[] bytes = GlobalEncoding.GetBytes(content);
                    LockFile(bytes, LockOperate.Lock);
                    fileStream.Write(bytes, 0, bytes.Length);
                    Debug.Log("д���ļ��ɹ�" + filepath);
                    return true;
                }
            }
            catch
            {
                Debug.Log("д���ļ�ʧ��" + filepath);
                return false;
            }
        }

        /// <summary>
        /// ɾ���ļ�
        /// </summary>
        public static bool DeleteFile(string path, string filename, string format)
        {

            string filepath = path + "/" + filename + "." + format;

            Debug.Log("ɾ���ļ�" + filepath);
            if (!Directory.Exists(path))
            {
                Debug.Log("·�������ڣ�ɾ���ļ�ʧ��" + filepath);
                return false;
            }

            if (!File.Exists(filepath))
            {
                Debug.Log("�ļ������ڣ�ɾ���ļ�ʧ��" + filepath);
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
                Debug.Log("ɾ���ļ�ʧ��" + filepath);
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

