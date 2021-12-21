using System.Collections;
using UnityEngine;

namespace Spg
{
    public class Player : MonoBehaviour
    {
        private Vector3 target;
        private float moveSpeed = 1000f;
        private Animator Anime;

        private void Start()
        {
            target = transform.position;
            
        }

        private void Awake()
        {
            Anime = GetComponent<Animator>();

            EventManager.Instance.AddListener(Consts.E_PlayerRun, Move);
            EventManager.Instance.AddListener(Consts.E_GoToStart, GoToStart);
        }

        public void Move(object step)
        {
            if (int.TryParse(step.ToString(), out int Step))
            {
                StartCoroutine(MoveToPosition(Step));
            }
        }

        public void GoToStart(object obj)
        {
            if (transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }
            target = GameData.Instance.GetStartGird();
            transform.position = target;
        }

        IEnumerator MoveToPosition(int step)
        {
            Anime.SetBool("isRun", true);
            while (step != 0)
            {
                if (step > 0)
                {
                    target = GameData.Instance.GetNextGird();
                    --step;
                }
                if (step < 0)
                {
                    target = GameData.Instance.GetPreGird();
                    ++step;
                }
                if ((target.x > transform.position.x && transform.localScale.x < 0) || (target.x < transform.position.x && transform.localScale.x > 0))
                {
                    transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                }
                while (!transform.position.Equals(target))
                {
                    transform.position = Vector3.MoveTowards(transform.localPosition, target, moveSpeed * Time.deltaTime);
                    yield return 0;
                }
            }
            Anime.SetBool("isRun", false);
            yield return new WaitForSeconds(0.1f);
            EventManager.Instance.SendMsg(GameData.Instance.GetCurrentEvent().Command);
        }
    }
}