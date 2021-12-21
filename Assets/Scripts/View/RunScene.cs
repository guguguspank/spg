using System.Collections;
using UnityEngine;

namespace Spg
{
    public class RunScene : MonoBehaviour
    {

        private void Awake()
        {
            StartCoroutine(Init());
        }

        public IEnumerator Init()
        {
            GameData.Instance.Init();
            Map map = new Map();
            map.Generate();
            RegisterEvent();
            yield return null;
        }

        private void RegisterEvent()
        {
            EventManager.Instance.AddListener(Consts.E_Sp, Events.Instance.Sp);
            EventManager.Instance.AddListener(Consts.E_Move, Events.Instance.Move);
            EventManager.Instance.AddListener(Consts.E_Num, Events.Instance.Num);
            EventManager.Instance.AddListener(Consts.E_BackToStart, Events.Instance.BackStart);
            EventManager.Instance.AddListener(Consts.E_Otk, Events.Instance.Otk);
            EventManager.Instance.AddListener(Consts.E_Diy, Events.Instance.Diy);
            EventManager.Instance.AddListener(Consts.E_NoHandle, Events.Instance.NoHandle);
            EventManager.Instance.AddListener(Consts.E_AddEffect, Events.Instance.AddEffect);
        }
    }
}
