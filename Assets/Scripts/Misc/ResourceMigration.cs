using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

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

        /// <summary>
        /// 首次运行迁移资源
        /// </summary>
        /// <returns></returns>
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
                PlayerPrefs.SetInt("isMigrate", 1);
            }
        }

        /// <summary>
        /// 尝试从StreamingAssets更新本地数据
        /// </summary>
        /// <returns></returns>
        public IEnumerator UpdateStreamingAssets()
        {
            var request = UnityWebRequest.Get(Path.Combine(Application.streamingAssetsPath, "Config", "GameConfig.yaml"));
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
            }
            else
            {
                string content = request.downloadHandler.text;
                GameConfig config = Config.LoadYamlStream<GameConfig>(content);
                if (PlayerPrefs.GetInt("ConfigVersion", 0) < config.ConfigVersion)
                {
                    RuntimeData.Instance.Conf.ConfigVersion = config.ConfigVersion;
                    Config.SaveYaml(RuntimeData.Instance.Conf, Path.Combine(Application.persistentDataPath, "Config/GameConfig.yaml"));
                    yield return StartCoroutine(UpdateConfig("Config/SpTool.yaml", RuntimeData.Instance.SpTools));
                    yield return StartCoroutine(UpdateConfig("Config/SpPosture.yaml", RuntimeData.Instance.SpPostures));
                    yield return StartCoroutine(UpdateEvent());
                    PlayerPrefs.SetInt("ConfigVersion", config.ConfigVersion);
                }
            }
        }

        private IEnumerator UpdateConfig<T>(string path, List<T> OldConfig, bool needSave = true) where T : IConfig
        {
            var request = UnityWebRequest.Get(Path.Combine(Application.streamingAssetsPath, path));
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
            }
            else
            {
                var content = request.downloadHandler.text;
                List<T> NewConfigList = Config.LoadYamlStream<List<T>>(content);
                Dictionary<string, T> NewConfigDict = NewConfigList.ToDictionary(key => key.Id, value => value);
                foreach (var item in OldConfig)
                {
                    if (item.Ignore)
                    {
                        NewConfigDict[item.Id] = item;
                    }
                }
                NewConfigList = NewConfigDict.Values.ToList();
                OldConfig = NewConfigList;
                if (needSave)
                {
                    Config.SaveYaml(OldConfig, Path.Combine(Application.persistentDataPath, path));
                }
            }
        }

        private IEnumerator UpdateEvent()
        {
            List<Event> temp = Config.LoadYaml<List<Event>>(Path.Combine(Application.streamingAssetsPath, "Config/Event/MoveEvent.yaml"));
            yield return StartCoroutine(UpdateConfig("Config/Event/MoveEvent.yaml", temp, false));
            Config.SaveYaml(temp, Path.Combine(Application.persistentDataPath, "Config/Event/MoveEvent.yaml"));
            RuntimeData.Instance.Events = temp;

            temp = Config.LoadYaml<List<Event>>(Path.Combine(Application.streamingAssetsPath, "Config/Event/SpEvent.yaml"));
            yield return StartCoroutine(UpdateConfig("Config/Event/SpEvent.yaml", temp, false));
            Config.SaveYaml(temp, Path.Combine(Application.persistentDataPath, "Config/Event/SpEvent.yaml"));
            RuntimeData.Instance.Events.AddRange(temp);

            if (RuntimeData.Instance.Conf.Extra)
            {
                temp = Config.LoadYaml<List<Event>>(Path.Combine(Application.streamingAssetsPath, "Config/Event/ExtraEvent.yaml"));
                yield return StartCoroutine(UpdateConfig("Config/Event/ExtraEvent.yaml", temp, false));
                Config.SaveYaml(temp, Path.Combine(Application.persistentDataPath, "Config/Event/ExtraEvent.yaml"));
                RuntimeData.Instance.Events.AddRange(temp);
            }
        }
    }
}
