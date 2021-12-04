using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Spg
{
    public class Menu : MonoBehaviour
    {
        GameObject SettingPanel { get; set; }
        GameObject Error { get; set; }
        InputField GirdCount { set; get; }
        InputField EventProb { set;get; }
        InputField SpEventProb { get; set; }
        Text ErrorText { get; set; }

        private void Awake()
        {
            GameObject.Find("UI Canvas/StartGame").GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene("run"));
            GameObject.Find("UI Canvas/Setting").GetComponent<Button>().onClick.AddListener(ShowSetting);
            GameObject.Find("UI Canvas/SettingPanel/Image/sure").GetComponent<Button>().onClick.AddListener(SettingOk);
            GameObject.Find("UI Canvas/Error/sure").GetComponent<Button>().onClick.AddListener(() => Error.SetActive(false));
            SettingPanel = GameObject.Find("UI Canvas/SettingPanel");
            GirdCount = GameObject.Find("UI Canvas/SettingPanel/Image/GirdCount").GetComponent<InputField>();
            EventProb = GameObject.Find("UI Canvas/SettingPanel/Image/EventProb").GetComponent<InputField>();
            SpEventProb = GameObject.Find("UI Canvas/SettingPanel/Image/SpEventProb").GetComponent<InputField>();

            Error = GameObject.Find("UI Canvas/Error");
            ErrorText = GameObject.Find("UI Canvas/Error/Text").GetComponent<Text>();

            SettingPanel.SetActive(false);
            Error.SetActive(false);
        }

        private void ShowSetting()
        {
            GirdCount.text = RuntimeData.Instance.Conf.GirdCount.ToString();
            GirdCount.textComponent.text = RuntimeData.Instance.Conf.GirdCount.ToString();
            EventProb.text = RuntimeData.Instance.Conf.Event.ToString();
            SpEventProb.text = RuntimeData.Instance.Conf.SpEvent.ToString();
            SettingPanel.SetActive(true);
        }

        private void SettingOk()
        {

            int tmp = int.Parse(GirdCount.text);
            if (tmp >= 50 && tmp <= 500)
            {
                RuntimeData.Instance.Conf.GirdCount = tmp;
            }
            else
            {
                ErrorText.text = "生成格子数目应该在50-500之间。";
                Error.SetActive(true);
            }
            RuntimeData.Instance.Conf.Event = int.Parse(EventProb.text);
            RuntimeData.Instance.Conf.SpEvent = int.Parse(SpEventProb.text);
            
            Config.SaveYaml<GameConfig>(RuntimeData.Instance.Conf, Path.Combine(Application.persistentDataPath, "Config", "GameConfig.yaml"));

            SettingPanel.SetActive(false);
        }
    }
}
