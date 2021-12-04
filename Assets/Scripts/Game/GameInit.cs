using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Spg
{
    /// <summary>
    /// ”Œœ∑≥ı ºªØ
    /// </summary>
    public class GameInit : MonoBehaviour
    {
        private void Start()
        {
            Init();
        }

        public void Init()
        {
            GameObject.Find("Wizard").GetComponent<Animator>().SetBool("isRun", true);

            ResourceMigration.MigrateStreamingAssets();

            RuntimeData.Instance.Init();
            RandomGenerator.Instance.Init();
            EventHandler.Instance.Init();

            SceneManager.LoadScene("menu");
        }
    }
}
