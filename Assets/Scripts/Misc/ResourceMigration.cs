using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Spg
{
    /// <summary>
    /// 迁移StreamingAssets目录下资源到应用目录下
    /// </summary>
    public class ResourceMigration : MonoBehaviour
    {
        private readonly string[] FilePath =
        {
            "Readme.md",
            "Config/GameConfig.yaml",
            "Config/SpPosture.yaml",
            "Config/SpTool.yaml",
            "Config/Event/MoveEvent.yaml",
            "Config/Event/SpEvent.yaml",
            "Config/Event/ExtraEvent.yaml"
        };

        public IEnumerator MigrateStreamingAssets()
        {
            if (PlayerPrefs.GetInt("isMigrate", 0) == 0)
            {
                string path = Application.persistentDataPath;
                if (!Directory.Exists(Path.Combine(path, "Config")))
                {
                    Directory.CreateDirectory(Path.Combine(path, "Config"));
                }
                if (!Directory.Exists(Path.Combine(path, "Config/Event")))
                {
                    Directory.CreateDirectory(Path.Combine(path, "Config/Event"));
                }
                foreach (var fp in FilePath)
                {
                    var request = UnityWebRequest.Get(Path.Combine(Application.streamingAssetsPath, fp));
                    yield return request.SendWebRequest();
                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        Debug.LogError(request.error);
                    }
                    else
                    {
                        string content = request.downloadHandler.text;
                        string savePath = Path.Combine(path, fp);
                        File.WriteAllText(savePath, content, System.Text.Encoding.UTF8);
                    }
                }
                yield return null;
                // PlayerPrefs.SetInt("isMigrate", 1);
            }
        }
      
    }
}
