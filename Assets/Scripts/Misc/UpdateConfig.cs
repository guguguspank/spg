using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace Spg
{
    public class UpdateConfig : MonoBehaviour
    {
        /// <summary>
        /// 尝试从StreamingAssets更新本地数据
        /// </summary>
        /// <returns></returns>
        public IEnumerator UpdateStreamingAssets()
        {
            yield return StartCoroutine(UpdateConfigs(Application.streamingAssetsPath));
        }

        /// <summary>
        /// 尝试从Github更新本地数据
        /// </summary>
        /// <returns></returns>
        public IEnumerator UpdateGithub()
        {
            yield return StartCoroutine(UpdateConfigs(@"https://raw.githubusercontent.com/guguguspank/spg/main/Assets/StreamingAssets"));
        }

        /// <summary>
        /// 更新配置
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private IEnumerator UpdateConfigs(string path)
        {
            var request = UnityWebRequest.Get(Path.Combine(path, "Config", "GameConfig.yaml"));
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
                    yield return StartCoroutine(UC(path, "Config/SpTool.yaml", RuntimeData.Instance.SpTools));
                    yield return StartCoroutine(UC(path, "Config/SpPosture.yaml", RuntimeData.Instance.SpPostures));
                    yield return StartCoroutine(UpdateEvent(path));
                    PlayerPrefs.SetInt("ConfigVersion", config.ConfigVersion);
                }
            }
        }

        private IEnumerator UC<T>(string basepath, string path, List<T> OldConfig, bool needSave = true) where T : IConfig
        {
            var request = UnityWebRequest.Get(Path.Combine(basepath, path));
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

        private IEnumerator UpdateEvent(string path)
        {
            RuntimeData.Instance.Events = new List<Event>();
            List<Event> temp = Config.LoadYaml<List<Event>>(Path.Combine(Application.persistentDataPath, "Config/Event/MoveEvent.yaml"));
            yield return StartCoroutine(UC(path, "Config/Event/MoveEvent.yaml", temp, false));
            Config.SaveYaml(temp, Path.Combine(Application.persistentDataPath, "Config/Event/MoveEvent.yaml"));
            foreach (var item in temp)
            {
                if (item.Args.TryGetValue("Step", out string step) && int.TryParse(step, out int tmp))
                {
                    item.Step = tmp;
                }
                RuntimeData.Instance.Events.Add(item);
            }

            temp = Config.LoadYaml<List<Event>>(Path.Combine(Application.persistentDataPath, "Config/Event/SpEvent.yaml"));
            yield return StartCoroutine(UC(path, "Config/Event/SpEvent.yaml", temp, false));
            Config.SaveYaml(temp, Path.Combine(Application.persistentDataPath, "Config/Event/SpEvent.yaml"));
            RuntimeData.Instance.Events.AddRange(temp);
            foreach (var item in temp)
            {
                if (item.Args.TryGetValue("Step", out string step) && int.TryParse(step, out int tmp))
                {
                    item.Step = tmp;
                }
                RuntimeData.Instance.Events.Add(item);
            }

            if (RuntimeData.Instance.Conf.Extra)
            {
                temp = Config.LoadYaml<List<Event>>(Path.Combine(Application.persistentDataPath, "Config/Event/ExtraEvent.yaml"));
                yield return StartCoroutine(UC(path, "Config/Event/ExtraEvent.yaml", temp, false));
                Config.SaveYaml(temp, Path.Combine(Application.persistentDataPath, "Config/Event/ExtraEvent.yaml"));
                foreach (var item in temp)
                {
                    if (item.Args.TryGetValue("Step", out string step) && int.TryParse(step, out int tmp))
                    {
                        item.Step = tmp;
                    }
                    RuntimeData.Instance.Events.Add(item);
                }
            }
        }
    }
}