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

        private bool needHandle;
        private Event CurrentEvent;

        private void Awake()
        {
            needHandle = false;
            Panel = transform.Find("Mask").gameObject;
            text = transform.Find("Mask/Background/Text").GetComponent<TextMeshProUGUI>();
            transform.Find("Mask/Background/Button").GetComponent<Button>().onClick.AddListener(OnButtonClick);

            EventManager.Instance.AddListener(Consts.E_ShowMsg, ShowMsg);
            EventManager.Instance.AddListener(Consts.E_ShowMsgAndMove, ShowMsgAndMove);

            Panel.SetActive(false);
        }

        private void OnButtonClick()
        {
            Panel.SetActive(false);
            if (needHandle)
            {
                needHandle = false;
                if (CurrentEvent.Command.Equals(Consts.E_Move))
                {
                    EventManager.Instance.SendMsg(Consts.E_PlayerRun, CurrentEvent.Step);
                }
                if (CurrentEvent.Command.Equals(Consts.E_BackToStart))
                {
                    EventManager.Instance.SendMsg(Consts.E_GoToStart);
                }
            }
        }

        public void ShowMsg(object msg)
        {
            Panel.SetActive(true);
            text.text = msg.ToString();
        }

        public void ShowMsgAndMove(object obj)
        {
            if (obj is Event)
            {
                needHandle = true;
                CurrentEvent = obj as Event;
                ShowMsg(CurrentEvent.ShowMsg);
            }
        }
    }
}
