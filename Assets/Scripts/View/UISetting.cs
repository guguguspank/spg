using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Spg
{
    public class UISetting : MonoBehaviour
    {
        TMP_InputField input;
        GameObject Panel;

        private void Awake()
        {
            Panel = transform.Find("SettingPanel").gameObject;

            transform.Find("SettingButton").GetComponent<Button>().onClick.AddListener(OpenSettingPanel);
            transform.Find("SettingPanel/Image/sure").GetComponent<Button>().onClick.AddListener(SaveConfig);
            input = transform.Find("SettingPanel/Image/GirdCountInput").GetComponent<TMP_InputField>();
            input.onValueChanged.AddListener(OnCountChange);

            Panel.SetActive(false);
        }

        private void OpenSettingPanel()
        {
            Panel.SetActive(true);
            input.text = RuntimeData.Instance.Conf.GirdCount.ToString();
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