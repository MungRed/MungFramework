#if UNITY_EDITOR
using UnityEditor;
namespace MungFramework.EditorExtension
{
    public class FileCreater : Editor
    {

        //右键菜单创建.md文件
        [MenuItem("Assets/MungFramework/文件/创建MD文件")]
        public static void CreateMarkdownFile()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path == "")
            {
                path = "Assets";
            }
            else if (System.IO.Path.GetExtension(path) != "")
            {
                path = path.Replace(System.IO.Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }
            string fileName = "NewMarkdownFile.md";
            string fullPath = AssetDatabase.GenerateUniqueAssetPath(path + "/" + fileName);
            System.IO.File.WriteAllText(fullPath, "");
            AssetDatabase.Refresh();
        }
    }
}
#endif
