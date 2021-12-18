using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Spg
{
    public class UIFinish : MonoBehaviour
    {
        private GameObject FinishPanel;

        private void Awake()
        {
            FinishPanel = transform.Find("Panel").gameObject;
            FinishPanel.transform.Find("FinishPanel/ok").GetComponent<Button>().onClick.AddListener(LeaveGame);
            EventManager.Instance.AddListener(Consts.E_GmaeFinish, GameFinish);

            FinishPanel.SetActive(false);
        }

        private void LeaveGame()
        {
            EventManager.Instance.Clear();
            SceneManager.LoadScene("menu");
        }

        public void GameFinish(object obj)
        {
            FinishPanel.SetActive(true);
        }
    }
}