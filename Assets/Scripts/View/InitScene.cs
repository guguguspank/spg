using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Spg
{
    /// <summary>
    /// 游戏初始化
    /// </summary>
    [RequireComponent(typeof(ResourceMigration))]
    public class InitScene : MonoBehaviour
    {
        private TextMeshProUGUI Loading;
        private string[] LoadText = { "Loading", "Loading.", "Loading..", "Loading...", "Loading...." };
        private int DotCount = 0;
        private float WaitTime = 0;

        private void Start()
        {
            Loading = GameObject.Find("Canvas/Loading").GetComponent<TextMeshProUGUI>();
            GameObject.Find("Wizard").GetComponent<Animator>().SetBool("isRun", true);
            StartCoroutine(Init());
        }

        private void Update()
        {
            WaitTime += Time.deltaTime;
            if (WaitTime >= 0.5)
            {
                WaitTime = 0;
                DotCount = (DotCount + 1) % 5;
                Loading.text = LoadText[DotCount];
            }
        }

        IEnumerator Init()
        {
            ResourceMigration rs = GameObject.Find("Canvas").GetComponent<ResourceMigration>();
            yield return StartCoroutine(rs.MigrateStreamingAssets());
            yield return StartCoroutine(RuntimeData.Instance.Init());
            yield return StartCoroutine(rs.UpdateStreamingAssets());
            yield return StartCoroutine(RandomGenerator.Instance.Init());
            
            SceneManager.LoadScene("menu");
        }
    }
}
