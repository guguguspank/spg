using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Spg
{
    /// <summary>
    /// 地图
    /// </summary>
    public class Map: MonoSingleton<Map>
    {
        public List<GameObject> GirdList { get; set; }

        public void Generate(int count)
        {
            if (count < 50)
            {
                count = 50;
            }
            if (count > 500)
            {
                count = 500;
            }

            GirdList = new List<GameObject>(count);

            GameObject SpGird = Resources.Load<GameObject>("Prefabs/SpGird");
            GameObject Clap = Resources.Load<GameObject>("Prefabs/Clap");
            GameObject Parent = GameObject.Find("ChessBoard Canvas/Background/ChessBoard");
            Transform ParentTransform = Parent.transform;
            float width = Parent.GetComponent<RectTransform>().rect.width;
            float height = Parent.GetComponent<RectTransform>().rect.height;
            Vector3 pos = new Vector3(210 - width / 2, 210 - height / 2, 0);
            bool toRight = true;
            int lineCount = 24;

            GameObject start = Gen(SpGird, ParentTransform, pos);
            GirdList.Add(start);
            start.transform.Find("Text").GetComponent<Text>().text = "起点";
            start.transform.Find("Text").GetComponent<Text>().fontSize = 180;
            pos.x += 420;

            for (int i = 0; i < count - 2; )
            {
                if (lineCount > 0)
                {
                    GameObject gameObject = Gen(SpGird, ParentTransform, pos);
                    GirdList.Add(gameObject);
                    gameObject.transform.Find("Text").GetComponent<Text>().text = GameData.Instance.Events[i].Name;
                    lineCount--;
                    if (lineCount > 0)
                    {
                        if (toRight)
                        {
                            pos.x += 420;
                        }
                        else
                        {
                            pos.x -= 420;
                        }
                    }
                    else
                    {
                        pos.y += 250;
                    }
                    i++;
                }
                else
                {
                    Gen(Clap, ParentTransform, pos);
                    pos.y += 250;
                    toRight = !toRight;
                    lineCount = 25;
                }
            }

            GameObject end = Gen(SpGird, ParentTransform, pos);
            GirdList.Add(end);
            end.transform.Find("Text").GetComponent<Text>().text = "终点";
            end.transform.Find("Text").GetComponent<Text>().fontSize = 180;
        }

        private GameObject Gen(GameObject c, Transform p, Vector3 pos)
        {
            GameObject gameObject = GameObject.Instantiate<GameObject>(c);
            gameObject.transform.SetParent(p);
            gameObject.transform.position = pos;
            return gameObject;
        }
    }
}