using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Spg
{
    public class UIDice : MonoBehaviour
    {
        GameObject DicePanel;
        Button SureButton;
        Animator Anime;
        Image Dice;

        private void Awake()
        {
            DicePanel = transform.Find("DicePanel").gameObject;
            SureButton = transform.Find("DicePanel/sure").GetComponent<Button>();
            Anime = transform.Find("DicePanel/Dice").GetComponent<Animator>();
            Anime.enabled = false;
            Dice = transform.Find("DicePanel/Dice").GetComponent<Image>();

            transform.Find("DiceButton").GetComponent<Button>().onClick.AddListener(OnDiceButtonClick);
            SureButton.onClick.AddListener(DiceFinish);

            SureButton.enabled = false;
            DicePanel.SetActive(false);
        }

        private void OnDiceButtonClick()
        {
            DicePanel.SetActive(true);
            var random = new System.Random(Guid.NewGuid().GetHashCode());
            float r = ((float)random.NextDouble() + 1);
            StartCoroutine(DicePlay(r));
        }

        private IEnumerator DicePlay(float t)
        {
            Anime.enabled = true;
            yield return new WaitForSeconds(t);
            Anime.enabled = false;
            SureButton.enabled = true;
        }

        private void DiceFinish()
        {
            SureButton.enabled = false;
            DicePanel.SetActive(false);
            EventManager.Instance.SendMsg(Consts.E_PlayerRun, int.Parse(Dice.sprite.name));
        }
    }
}
