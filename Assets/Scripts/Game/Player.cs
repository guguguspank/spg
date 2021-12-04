using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spg
{
    public class Player : MonoBehaviour
    {
        private Animator animator;
        private Vector3 target;
        private Vector3 right = new Vector3(120, 120, 1);
        private Vector3 left = new Vector3(-120, 120, 1);
        private float moveSpeed = 1000f;
        private Run run;

        public void Move(int step)
        {
            StartCoroutine(MoveToPosition(step, run.MoveFinish));
        }

        private void Start()
        {
            target = transform.position;
            run = GameObject.Find("Run").GetComponent<Run>();
            animator = GetComponent<Animator>();
        }

        IEnumerator MoveToPosition(int step, Action callback)
        {
            GameData data = GameData.Instance;
            Map map = Map.Instance;
            while (step != 0)
            {
                if (step > 0)
                {
                    ++data.CurrentGird;
                    ++data.CurrentEvent;
                    --step;
                }
                if (step < 0)
                {
                    --data.CurrentGird;
                    --data.CurrentEvent;
                    ++step;
                }
                if (data.CurrentGird >= RuntimeData.Instance.Conf.GirdCount)
                {
                    data.CurrentGird = RuntimeData.Instance.Conf.GirdCount - 1;
                }
                target = map.GirdList[data.CurrentGird].transform.position;
                if (target.x - transform.position.x > 0)
                {
                    transform.localScale = right;
                }
                if (target.x - transform.position.x < 0)
                {
                    transform.localScale = left;
                }
                while (!transform.position.Equals(target))
                {
                    animator.SetBool("isRun", true);
                    transform.position = Vector3.MoveTowards(transform.localPosition, target, moveSpeed * Time.deltaTime);
                    yield return 0;
                }
            }
            animator.SetBool("isRun", false);

            callback?.Invoke();
        }

        public void BackStart()
        {
            GameData.Instance.CurrentEvent = -1;
            GameData.Instance.CurrentGird = 0;
            transform.position = Map.Instance.GirdList[0].transform.position;
        }
    }
}