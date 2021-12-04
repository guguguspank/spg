using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Spg
{
    public class Run : MonoBehaviour
    {
        GameData data { get; set;}
        Map map { get; set; }

        GameObject Detail { get; set; }
        GameObject Sure { get; set; }
        GameObject Finish { get; set; }
        GameObject DicePanel { get; set; }
        GameObject Dice { get; set; }

        Animator animator { get; set; }

        Button DiceSure { get; set; }

        int step { get; set; }

        private void Awake()
        {
            data = GameData.Instance;
            map = Map.Instance;

            data.Init();
            map.Generate(RuntimeData.Instance.Conf.GirdCount);
            GameObject.Find("Player").transform.position = Map.Instance.GirdList[0].transform.position;

            GameObject.Find("UI Canvas/Button").GetComponent<Button>().onClick.AddListener(Back);
            GameObject.Find("UI Canvas/Sure/sure").GetComponent<Button>().onClick.AddListener(BackToMenu);
            GameObject.Find("UI Canvas/Finish/sure").GetComponent<Button>().onClick.AddListener(BackToMenu);
            GameObject.Find("UI Canvas/Sure/cancel").GetComponent<Button>().onClick.AddListener(Cancel);
            GameObject.Find("UI Canvas/DiceButton").GetComponent<Button>().onClick.AddListener(DiceStart);
            GameObject.Find("UI Canvas/Detail/Background/Button").GetComponent<Button>().onClick.AddListener(CloseDetail);
            DiceSure = GameObject.Find("UI Canvas/DicePanel/sure").GetComponent<Button>();
            DiceSure.onClick.AddListener(DiceClose);
            DiceSure.enabled = false;

            animator = GameObject.Find("UI Canvas/DicePanel/Dice").GetComponent<Animator>();
            animator.enabled = false;

            Detail = GameObject.Find("UI Canvas/Detail");
            Sure = GameObject.Find("UI Canvas/Sure");
            Finish = GameObject.Find("UI Canvas/Finish");
            DicePanel = GameObject.Find("UI Canvas/DicePanel");
            Dice = GameObject.Find("UI Canvas/DicePanel/Dice");
            Detail.SetActive(false);
            Sure.SetActive(false);
            Finish.SetActive(false);
            DicePanel.SetActive(false);
        }

        private void Back()
        {
            Sure.SetActive(true);
        }

        private void BackToMenu()
        {
            SceneManager.LoadScene("menu");
        }

        private void Cancel()
        {
            Sure.SetActive(false);
        }

        private void DiceStart()
        {
            DicePanel.SetActive(true);
            var random = new System.Random(Guid.NewGuid().GetHashCode());
            float r = ((float)random.NextDouble() + 1) * 1.5f;
            animator.enabled = true;
            Invoke(nameof(DiceOk), r);
        }

        private void DiceOk()
        {
            animator.enabled = false;
            DiceSure.enabled = true;
            step = int.Parse(Dice.GetComponent<Image>().sprite.name);
            
        }

        private void DiceClose()
        {
            DiceSure.enabled = false;
            DicePanel.SetActive(false);
            data.player.Move(step);
        }

        private void CloseDetail()
        {
            Detail.SetActive(false);
            if (data.Events[data.CurrentEvent].needHandle)
            {
                EventHandler.Instance.Execute(data.Events[data.CurrentEvent]);
            }
        }

        public void MoveFinish()
        {
            if (data.CurrentGird == RuntimeData.Instance.Conf.GirdCount - 1)
            {
                Finish.SetActive(true);
            }
            else
            {
                Detail.SetActive(true);
                Text t = GameObject.Find("UI Canvas/Detail/Background/Text").GetComponent<Text>();
                t.text = data.Events[data.CurrentEvent].ShowMsg;
                if (!data.Events[data.CurrentEvent].needHandle)
                {
                    foreach (var item in data.buff)
                    {
                        if (item.Value.Count > 0)
                        {
                            t.text += $"\n¸½¼ÓÒªÇó£º{item.Value.Effect}";
                            item.Value.Count--;
                        }
                    }
                }
            }
        }
    }
}
