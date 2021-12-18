using TMPro;
using UnityEngine;

namespace Spg
{
    /// <summary>
    /// 地图
    /// </summary>
    public class Map
    {
        private GameObject Gird = Resources.Load<GameObject>("Prefabs/Gird");
        private GameObject Clap = Resources.Load<GameObject>("Prefabs/Clap");
        private Transform Parent = GameObject.Find("ChessBoard").transform;

        public void Generate()
        {
            const float addX = 480;
            const float addY = 290;
            int count = RuntimeData.Instance.Conf.GirdCount;
            int lineCount = Mathf.CeilToInt(Mathf.Sqrt(count + 2));

            float posX = 0;
            float posY = 0;
            int currentLineCount = lineCount - 1;
            int direct = 1;

            Vector3 vector = new Vector3(posX, posY, 0);
            GameData.Instance.Girds[0].Pos = vector;
            GenPoint(vector).transform.Find("Text").GetComponent<TextMeshProUGUI>().text = GameData.Instance.Girds[0].Msg;
            posX += addX;

            for (int i = 1; i <= count; ++i)
            {
                vector = new Vector3(posX, posY, 0);
                GameData.Instance.Girds[i].Pos = vector;
                GenGird(vector).transform.Find("Text").GetComponent<TextMeshProUGUI>().text = GameData.Instance.Girds[i].Msg;
                currentLineCount--;

                if (currentLineCount == 0)
                {
                    currentLineCount = lineCount;
                    direct *= -1;
                    posY += addY;

                    vector = new Vector3(posX, posY, 0);
                    GenClap(vector);
                    posY += addY;
                }
                else
                {
                    posX += (addX * direct);
                }
            }

            vector = new Vector3(posX, posY, 0);
            GameData.Instance.Girds[count + 1].Pos = vector;
            GenPoint(vector).transform.Find("Text").GetComponent<TextMeshProUGUI>().text = GameData.Instance.Girds[count + 1].Msg;
        }

        private GameObject GenPoint(Vector3 vector)
        {
            GameObject gameObject = GenGird(vector);
            TextMeshProUGUI text = gameObject.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            text.fontSize = 200;
            text.alignment = TextAlignmentOptions.MidlineFlush;
            return gameObject;
        }

        private GameObject GenGird(Vector3 vector)
        {
            GameObject gameObject = GameObject.Instantiate<GameObject>(Gird);
            gameObject.transform.SetParent(Parent);
            gameObject.transform.position = vector;
            return gameObject;
        }

        private void GenClap(Vector3 vector)
        {
            GameObject gameObject = GameObject.Instantiate<GameObject>(Clap);
            gameObject.transform.SetParent(Parent);
            gameObject.transform.position = vector;
        }
    }
}