using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;

namespace Spg
{
    /// <summary>
    /// 迁移StreamingAssets目录下资源到应用目录下
    /// </summary>
    public static class ResourceMigration
    {
        private static readonly int Config_Version = 1;
        private static readonly string[] File_Path =
        {
            "Readme.md",
            "Config/Event.yaml",
            "Config/ExtraEvent.yaml",
            "Config/GameConfig.yaml",
            "Config/PlayMethod.yaml",
            "Config/Position.yaml",
            "Config/SpEvent.yaml",
            "Config/SpTool.yaml"
        };

        public static void MigrateStreamingAssets()
        {
            int version = PlayerPrefs.GetInt("ConfigVersion", 0);

            if (version < Config_Version)
            {
                // Migrate(Application.streamingAssetsPath, "");
                Migrate();
                PlayerPrefs.SetInt("ConfigVersion", Config_Version);
            }
        }

        private static void Migrate()
        {
            string path = Application.persistentDataPath;
            if (!Directory.Exists(Path.Combine(path, "Config")))
            {
                Directory.CreateDirectory(Path.Combine(path, "Config"));
            }
            foreach (var fp in File_Path)
            {
                var request = UnityWebRequest.Get(Path.Combine(Application.streamingAssetsPath, fp));
                request.SendWebRequest();
                while (!request.isDone) { }
                string content = request.downloadHandler.text;
                string savePath = Path.Combine(path, fp);
                File.WriteAllText(savePath, content, System.Text.Encoding.UTF8);
            }
        }
        
        /// <summary>
        /// 没错，傻逼安卓没法用
        /// </summary>
        /// <param name="basePath"></param>
        /// <param name="extraPath"></param>
        private static void Migrate(string basePath, string extraPath)
        {
            string path = Path.Combine(basePath, extraPath);
            DirectoryInfo root = new DirectoryInfo(path);
            foreach (DirectoryInfo info in root.GetDirectories())
            {
                string persistentDataPath = Path.Combine(Application.persistentDataPath, extraPath, info.Name);
                if (!Directory.Exists(persistentDataPath))
                {
                    Directory.CreateDirectory(persistentDataPath);
                }
                Migrate(basePath, Path.Combine(extraPath, info.Name));
            }
            foreach (FileInfo info in root.GetFiles())
            {
                if (!info.Extension.Equals(".meta"))
                {
                    var request = UnityWebRequest.Get(Path.Combine(path, info.FullName));
                    request.SendWebRequest();
                    while (!request.isDone) { }
                    string content = request.downloadHandler.text;
                    string savePath = Path.Combine(Application.persistentDataPath, extraPath, info.Name);
                    File.WriteAllText(savePath, content, System.Text.Encoding.UTF8);
                }
            }
        }
    }
}
