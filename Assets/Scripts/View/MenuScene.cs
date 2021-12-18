using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Spg
{
    public class MenuScene : MonoBehaviour
    {
        private void Awake()
        {
            GameObject.Find("UI Canvas/StartGame").GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene("run"));
        }
    }
}
