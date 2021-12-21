using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Spg
{
    [RequireComponent(typeof(UpdateConfig))]
    public class UISetting : MonoBehaviour
    {
        TMP_InputField input;
        GameObject Panel;
        GameObject UpdatePaenl;

        private void Awake()
        {
            Panel = transform.Find("SettingPanel").gameObject;

            GameObject.Find("UI Canvas/SettingButton").GetComponent<Button>().onClick.AddListener(OpenSettingPanel);
            transform.Find("SettingPanel/Image/sure").GetComponent<Button>().onClick.AddListener(SaveConfig);
            transform.Find("SettingPanel/Image/checkUpdate").GetComponent<Button>().onClick.AddListener(OnUpdateClick);
            UpdatePaenl = transform.Find("UpdatePanel").gameObject;
            input = transform.Find("SettingPanel/Image/GirdCountInput").GetComponent<TMP_InputField>();
            input.onValueChanged.AddListener(OnCountChange);

            UpdatePaenl.SetActive(false);
            Panel.SetActive(false);
        }

        private void OnUpdateClick()
        {
            UpdatePaenl.SetActive(true);
            StartCoroutine(UC());
        }

        private IEnumerator UC()
        {
            yield return StartCoroutine(transform.GetComponent<UpdateConfig>().UpdateGithub());
            transform.Find("SettingPanel/Image/ConfigVersion").GetComponent<TextMeshProUGUI>().text = $"{RuntimeData.Instance.Conf.ConfigVersion}(已尝试更新)";
            UpdatePaenl.SetActive(false);
        }

        private void OpenSettingPanel()
        {
            Panel.SetActive(true);
            input.text = RuntimeData.Instance.Conf.GirdCount.ToString();
            transform.Find("SettingPanel/Image/ConfigVersion").GetComponent<TextMeshProUGUI>().text = $"{RuntimeData.Instance.Conf.ConfigVersion}";
        }

        private void SaveConfig()
        {
            RuntimeData.Instance.Conf.GirdCount = int.Parse(input.text);
            Config.SaveYaml<GameConfig>(RuntimeData.Instance.Conf, Path.Combine(Application.persistentDataPath, "Config", "GameConfig.yaml"));
            Panel.SetActive(false);
        }

        public void OnCountChange(string value)
        {
            if (int.TryParse(value, out int num))
            {
                if (num < 1)
                {
                    num = 1;
                }
                if (num > 1105)
                {
                    num = 1105;
                }
                input.text = num.ToString();
            }
        }
    }
}