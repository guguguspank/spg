using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Spg
{
    public class UIBack : MonoBehaviour
    {
        private GameObject SurePanel;

        private void Awake()
        {
            SurePanel = transform.Find("Sure").gameObject;
            
            transform.Find("Button").GetComponent<Button>().onClick.AddListener(() => SurePanel.SetActive(true));
            SurePanel.transform.Find("cancel").GetComponent<Button>().onClick.AddListener(() => SurePanel.SetActive(false));

            SurePanel.transform.Find("sure").GetComponent<Button>().onClick.AddListener(LeaveGame);

            SurePanel.SetActive(false);
        }

        private void LeaveGame()
        {
            EventManager.Instance.Clear();
            SceneManager.LoadScene("menu");
        }
    }
}