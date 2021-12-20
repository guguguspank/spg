using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Spg
{
    /// <summary>
    /// TODO 还是找个大佬重写本功能吧
    /// </summary>
    public class UICustomize : MonoBehaviour
    {
        private GameObject CustomizePanel;
        private GameObject Prompt;
        private GameObject Content;

        private GameObject ConfigItem;
        private GameObject PostureDetail;
        private GameObject ToolDetail;

        private List<SpTool> MSpTools;
        private List<SpPosture> MPostures;
        private List<SpTool> NSpTools;
        private List<SpPosture> NSPostures;

        private List<GameObject> ToolButtons;
        private List<GameObject> PostureButtons;
        private Dictionary<GameObject, GameObject> EditDict;
        private Dictionary<GameObject, ToolBar> ToolDict;
        private Dictionary<GameObject, PostureBar> PostureDict;


        private void Awake()
        {
            CustomizePanel = transform.Find("Panel").gameObject;
            Prompt = transform.Find("Panel/Image/Scroll View/Viewport/Content/Text").gameObject;
            Content = transform.Find("Panel/Image/Scroll View/Viewport/Content").gameObject;

            ConfigItem = Resources.Load<GameObject>("Prefabs/ConfigItem");
            PostureDetail = Resources.Load<GameObject>("Prefabs/PostureDetail");
            ToolDetail = Resources.Load<GameObject>("Prefabs/ToolDetail");

            GameObject.Find("UI Canvas/CustomizeButton").GetComponent<Button>().onClick.AddListener(OnCustomizeButtonClick);
            transform.Find("Panel/Image/Sure").GetComponent<Button>().onClick.AddListener(OnSureClick);
            transform.Find("Panel/Image/Cancel").GetComponent<Button>().onClick.AddListener(OnCancelClick);
            transform.Find("Panel/Image/ToggleGroup/Prompt").GetComponent<Toggle>().onValueChanged.AddListener(ChangePanel);
            transform.Find("Panel/Image/ToggleGroup/Tool").GetComponent<Toggle>().onValueChanged.AddListener(ChangePanel);
            transform.Find("Panel/Image/ToggleGroup/Posture").GetComponent<Toggle>().onValueChanged.AddListener(ChangePanel);

            MSpTools = RuntimeData.Instance.SpTools;
            MPostures = RuntimeData.Instance.SpPostures;
            NSpTools = new List<SpTool>();
            NSPostures = new List<SpPosture>();

            ToolButtons = new List<GameObject>();
            PostureButtons = new List<GameObject>();
            EditDict = new Dictionary<GameObject, GameObject>();
            ToolDict = new Dictionary<GameObject, ToolBar>();
            PostureDict = new Dictionary<GameObject, PostureBar>();

            InitItem();

            CustomizePanel.SetActive(false);
        }

        private void InitItem()
        {
            foreach (var item in MSpTools)
            {
                Transform button = GameObject.Instantiate<GameObject>(ConfigItem).transform;
                button.SetParent(Content.transform);
                button.localPosition = Vector3.zero;
                button.localRotation = Quaternion.identity;
                button.localScale = Vector3.one;
                Transform panel = GameObject.Instantiate<GameObject>(ToolDetail).transform;
                panel.SetParent(Content.transform);
                panel.localPosition = Vector3.zero;
                panel.localRotation = Quaternion.identity;
                panel.localScale = Vector3.one;

                button.Find("Desc").GetComponent<TextMeshProUGUI>().text = item.ToString();
                button.GetComponent<Button>().onClick.AddListener(ShowDetail);

                panel.Find("NameInput").GetComponent<TMP_InputField>().text = item.Name;
                panel.Find("NameInput").GetComponent<TMP_InputField>().onEndEdit.AddListener(ToolTitle);
                panel.Find("DescInput").GetComponent<TMP_InputField>().text = item.Desc;
                panel.Find("DescInput").GetComponent<TMP_InputField>().onEndEdit.AddListener(ToolTitle);
                panel.Find("MinCountInput").GetComponent<TMP_InputField>().text = item.MinCount.ToString();
                panel.Find("MaxCountInput").GetComponent<TMP_InputField>().text = item.MaxCount.ToString();
                panel.Find("WeightInput").GetComponent<TMP_InputField>().text = item.Weight.ToString();
                panel.Find("WeightInput").GetComponent<TMP_InputField>().onEndEdit.AddListener(OnWeightEdit);
                panel.Find("EnableInput").GetComponent<Toggle>().isOn = item.Enable;
                panel.Find("IgnoreInput").GetComponent<Toggle>().isOn = item.Ignore;
                panel.Find("DiyInput").GetComponent<Toggle>().isOn = item.IsDiy;
                panel.Find("OtkInput").GetComponent<Toggle>().isOn = item.IsOtk;

                button.gameObject.SetActive(false);
                panel.gameObject.SetActive(false);
                ToolButtons.Add(button.gameObject);
                EditDict.Add(button.gameObject, panel.gameObject);
                ToolDict.Add(panel.gameObject, new ToolBar() { spTool = item, bar = button.gameObject});
            }

            foreach (var item in MPostures)
            {
                Transform button = GameObject.Instantiate<GameObject>(ConfigItem).transform;
                button.SetParent(Content.transform);
                button.localPosition = Vector3.zero;
                button.localRotation = Quaternion.identity;
                button.localScale = Vector3.one;
                Transform panel = GameObject.Instantiate<GameObject>(PostureDetail).transform;
                panel.SetParent(Content.transform);
                panel.localPosition = Vector3.zero;
                panel.localRotation = Quaternion.identity;
                panel.localScale = Vector3.one;

                button.Find("Desc").GetComponent<TextMeshProUGUI>().text = item.ToString();
                button.GetComponent<Button>().onClick.AddListener(ShowDetail);

                panel.Find("NameInput").GetComponent<TMP_InputField>().text = item.Name;
                panel.Find("NameInput").GetComponent<TMP_InputField>().onEndEdit.AddListener(PostureTitle);
                panel.Find("DescInput").GetComponent<TMP_InputField>().text = item.Desc;
                panel.Find("DescInput").GetComponent<TMP_InputField>().onEndEdit.AddListener(PostureTitle);
                panel.Find("WeightInput").GetComponent<TMP_InputField>().text = item.Weight.ToString();
                panel.Find("WeightInput").GetComponent<TMP_InputField>().onEndEdit.AddListener(OnWeightEdit);
                panel.Find("EnableInput").GetComponent<Toggle>().isOn = item.Enable;
                panel.Find("IgnoreInput").GetComponent<Toggle>().isOn = item.Ignore;
                panel.Find("DiyInput").GetComponent<Toggle>().isOn = item.IsDiy;
                panel.Find("OtkInput").GetComponent<Toggle>().isOn = item.IsOtk;

                button.gameObject.SetActive(false);
                panel.gameObject.SetActive(false);
                PostureButtons.Add(button.gameObject);
                EditDict.Add(button.gameObject, panel.gameObject);
                PostureDict.Add(panel.gameObject, new PostureBar() { spPosture = item, bar = button.gameObject});
            }
        }

        private void OnWeightEdit(string value)
        {
            var input = EventSystem.current.currentSelectedGameObject;
            if (int.TryParse(value, out int num))
            {
                if (num < 1)
                {
                    num = 1;
                }
                if (num > 100)
                {
                    num = 100;
                }
                input.GetComponent<TMP_InputField>().text = num.ToString();
            }
        }

        private void ToolTitle(string value)
        {
            var parent = EventSystem.current.currentSelectedGameObject.transform.parent;
            var name = parent.Find("NameInput").GetComponent<TMP_InputField>().text;
            var desc = parent.Find("DescInput").GetComponent<TMP_InputField>().text;
            ToolDict[parent.gameObject].bar.transform.Find("Desc").GetComponent<TextMeshProUGUI>().text = $"{name}({desc})";
        }

        private void PostureTitle(string value)
        {
            var parent = EventSystem.current.currentSelectedGameObject.transform.parent;
            var name = parent.Find("NameInput").GetComponent<TMP_InputField>().text;
            var desc = parent.Find("DescInput").GetComponent<TMP_InputField>().text;
            PostureDict[parent.gameObject].bar.transform.Find("Desc").GetComponent<TextMeshProUGUI>().text = $"{name}({desc})";
        }

        private void OnCustomizeButtonClick()
        {
            CustomizePanel.SetActive(true);
        }

        private void OnSureClick()
        {
            foreach (var item in ToolDict)
            {
                SpTool tool = item.Value.spTool;
                Transform panel = item.Key.transform;
                tool.Name = panel.Find("NameInput").GetComponent<TMP_InputField>().text;
                tool.Desc = panel.Find("DescInput").GetComponent<TMP_InputField>().text;
                tool.MinCount = int.Parse(panel.Find("MinCountInput").GetComponent<TMP_InputField>().text);
                tool.MaxCount = int.Parse(panel.Find("MaxCountInput").GetComponent<TMP_InputField>().text);
                tool.Weight = int.Parse(panel.Find("WeightInput").GetComponent<TMP_InputField>().text);
                tool.Enable = panel.Find("EnableInput").GetComponent<Toggle>().isOn;
                tool.Ignore = panel.Find("IgnoreInput").GetComponent<Toggle>().isOn;
                if (panel.Find("DiyInput").GetComponent<Toggle>().isOn ^ tool.IsDiy)
                {
                    if (tool.IsDiy)
                    {
                        tool.Tag.Remove("diy");
                    }
                    else
                    {
                        tool.Tag.Add("diy");
                    }
                }
                if (panel.Find("OtkInput").GetComponent<Toggle>().isOn ^ tool.IsOtk)
                {
                    if (tool.IsOtk)
                    {
                        tool.Tag.Remove("otk");
                    }
                    else
                    {
                        tool.Tag.Add("otk");
                    }
                }
            }
            Config.SaveYaml<List<SpTool>>(RuntimeData.Instance.SpTools, Path.Combine(Application.persistentDataPath, "Config/SpTool.yaml"));
            StartCoroutine(RandomGenerator.Instance.UpdateTool());

            foreach (var item in PostureDict)
            {
                SpPosture posture = item.Value.spPosture;
                Transform panel = item.Key.transform;
                posture.Name = panel.Find("NameInput").GetComponent<TMP_InputField>().text;
                posture.Desc = panel.Find("DescInput").GetComponent<TMP_InputField>().text;
                posture.Weight = int.Parse(panel.Find("WeightInput").GetComponent<TMP_InputField>().text);
                posture.Enable = panel.Find("EnableInput").GetComponent<Toggle>().isOn;
                posture.Ignore = panel.Find("IgnoreInput").GetComponent<Toggle>().isOn;
                if (panel.Find("DiyInput").GetComponent<Toggle>().isOn ^ posture.IsDiy)
                {
                    if (posture.IsDiy)
                    {
                        posture.Tag.Remove("diy");
                    }
                    else
                    {
                        posture.Tag.Add("diy");
                    }
                }
                if (panel.Find("OtkInput").GetComponent<Toggle>().isOn ^ posture.IsOtk)
                {
                    if (posture.IsOtk)
                    {
                        posture.Tag.Remove("otk");
                    }
                    else
                    {
                        posture.Tag.Add("otk");
                    }
                }
            }
            Config.SaveYaml<List<SpPosture>>(RuntimeData.Instance.SpPostures, Path.Combine(Application.persistentDataPath, "Config/SpPosture.yaml"));
            StartCoroutine(RandomGenerator.Instance.UpdatePosture());

            SetDefaultPanel();
            CustomizePanel.SetActive(false);
        }

        private void OnCancelClick()
        {
            foreach (var item in ToolDict)
            {
                SpTool tool = item.Value.spTool;
                Transform panel = item.Key.transform;
                panel.Find("NameInput").GetComponent<TMP_InputField>().text = tool.Name;
                panel.Find("DescInput").GetComponent<TMP_InputField>().text = tool.Desc;
                panel.Find("MinCountInput").GetComponent<TMP_InputField>().text = tool.MinCount.ToString();
                panel.Find("MaxCountInput").GetComponent<TMP_InputField>().text = tool.MaxCount.ToString();
                panel.Find("WeightInput").GetComponent<TMP_InputField>().text = tool.Weight.ToString();
                panel.Find("EnableInput").GetComponent<Toggle>().isOn = tool.Enable;
                panel.Find("IgnoreInput").GetComponent<Toggle>().isOn = tool.Ignore;
                panel.Find("DiyInput").GetComponent<Toggle>().isOn = tool.IsDiy;
                panel.Find("OtkInput").GetComponent<Toggle>().isOn = tool.IsOtk;
                item.Value.bar.transform.Find("Desc").GetComponent<TextMeshProUGUI>().text = tool.ToString();
            }

            foreach (var item in PostureDict)
            {
                SpPosture posture = item.Value.spPosture;
                Transform panel = item.Key.transform;
                panel.Find("NameInput").GetComponent<TMP_InputField>().text = posture.Name;
                panel.Find("DescInput").GetComponent<TMP_InputField>().text = posture.Desc;
                panel.Find("WeightInput").GetComponent<TMP_InputField>().text = posture.Weight.ToString();
                panel.Find("EnableInput").GetComponent<Toggle>().isOn = posture.Enable;
                panel.Find("IgnoreInput").GetComponent<Toggle>().isOn = posture.Ignore;
                panel.Find("DiyInput").GetComponent<Toggle>().isOn = posture.IsDiy;
                panel.Find("OtkInput").GetComponent<Toggle>().isOn = posture.IsOtk;
                item.Value.bar.transform.Find("Desc").GetComponent<TextMeshProUGUI>().text = posture.ToString();
            }

            SetDefaultPanel();
            CustomizePanel.SetActive(false);
        }

        private void SetDefaultPanel()
        {
            transform.Find("Panel/Image/ToggleGroup/Prompt").GetComponent<Toggle>().isOn = true;
            Prompt.SetActive(true);
            foreach (var item in ToolButtons)
            {
                item.SetActive(false);
                EditDict[item].SetActive(false);
            }
            foreach (var item in PostureButtons)
            {
                item.SetActive(false);
                EditDict[item].SetActive(false);
            }
        }

        private void ChangePanel(bool tag)
        {
            if (tag)
            {
                var toggle = EventSystem.current.currentSelectedGameObject;
                switch (toggle.name)
                {
                    case "Prompt":
                        Prompt.SetActive(true);
                        foreach (var item in ToolButtons)
                        {
                            item.SetActive(false);
                            EditDict[item].SetActive(false);
                        }
                        foreach (var item in PostureButtons)
                        {
                            item.SetActive(false);
                            EditDict[item].SetActive(false);
                        }
                        break;
                    case "Tool":
                        foreach (var item in ToolButtons)
                        {
                            item.SetActive(true);
                        }
                        Prompt.SetActive(false);
                        foreach (var item in PostureButtons)
                        {
                            item.SetActive(false);
                            EditDict[item].SetActive(false);
                        }
                        break;
                    case "Posture":
                        foreach (var item in PostureButtons)
                        {
                            item.SetActive(true);
                        }
                        Prompt.SetActive(false);
                        foreach (var item in ToolButtons)
                        {
                            item.SetActive(false);
                            EditDict[item].SetActive(false);
                        }
                        break;
                }
            }    
        }

        private void ShowDetail()
        {
            var button = EventSystem.current.currentSelectedGameObject;
            button.transform.Find("Prompt").GetComponent<TextMeshProUGUI>().text = "点击收起";
            EditDict[button].SetActive(true);
            button.GetComponent<Button>().onClick.RemoveListener(ShowDetail);
            button.GetComponent<Button>().onClick.AddListener(HideDetail);
        }

        private void HideDetail()
        {
            var button = EventSystem.current.currentSelectedGameObject;
            button.transform.Find("Prompt").GetComponent<TextMeshProUGUI>().text = "点击展开";
            EditDict[button].SetActive(false);
            button.GetComponent<Button>().onClick.RemoveListener(HideDetail);
            button.GetComponent<Button>().onClick.AddListener(ShowDetail);
        }
    }

    public class ToolBar
    {
        public SpTool spTool;
        public GameObject bar;
    }

    public class PostureBar
    {
        public SpPosture spPosture;
        public GameObject bar;
    }
}