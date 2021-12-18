using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Spg
{
    public class UIDetail : MonoBehaviour
    {
        GameObject Panel;
        TextMeshProUGUI text;

        private void Awake()
        {
            Panel = transform.Find("Mask").gameObject;
            text = transform.Find("Mask/Background/Text").GetComponent<TextMeshProUGUI>();
            transform.Find("Mask/Background/Button").GetComponent<Button>().onClick.AddListener(() => Panel.SetActive(false));

            EventManager.Instance.AddListener(Consts.E_ShowMsg, ShowMsg);

            Panel.SetActive(false);
        }

        public void ShowMsg(object msg)
        {
            Panel.SetActive(true);
            text.text = msg.ToString();
        }
    }
}
