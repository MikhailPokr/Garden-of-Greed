using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GardenOld
{


    public class Notice : MonoBehaviour
    {
        public GameObject notice;
        public Sprite coin;
        public Sprite stamina;
        public Sprite fire;

        public void ForStamina()
        {
            Show(1, "������� �������: "+World.stamina.ToString());
        }
        public void Show(int type, string value)
        {
            GameObject g = Instantiate(notice);
            Vector2 pos = Input.mousePosition;
            g.transform.SetParent(gameObject.transform);

            if (type == 0)
            {
                g.transform.position = new Vector2(pos.x + Random.Range(-10f, 10f), pos.y + Random.Range(-10f, 10f));
                if (!World.GreedMode)
                {
                    g.GetComponentInChildren<Image>().sprite = coin;
                    g.GetComponent<Text>().color = new Color(1, 0.8f, 0);
                }
                else
                {
                    g.GetComponentInChildren<Image>().sprite = coin;
                    g.GetComponentInChildren<Image>().color = new Color(1, 0, 0);
                    g.GetComponent<Text>().color = new Color(1, 0, 0);
                }

            }
            else if (type == 1)
            {
                g.transform.position = new Vector2(pos.x + Random.Range(-100f, -80f), pos.y + Random.Range(-10f, 10f));
                g.GetComponentInChildren<Image>().sprite = stamina;
                g.GetComponentInChildren<Image>().color = new Color(0.5f, 0.5f, 1);
                g.GetComponent<Text>().color = new Color(0.5f, 0.5f, 1);
            }
            else
            {
                g.transform.position = new Vector2(pos.x + Random.Range(-10f, -10f), pos.y + Random.Range(-70f, -50f));
                g.GetComponentInChildren<Image>().sprite = fire;
                g.GetComponentInChildren<Image>().color = new Color(1, 0.4f, 0);
                g.GetComponent<Text>().color = new Color(1, 0.4f, 0);
            }
            g.GetComponent<Text>().text = value[0] == '-' || value[0] == '�' ? value : "+" + value;
        }
        public void Show(int type, string value, Vector3 pos)
        {
            GameObject g = Instantiate(notice);
            g.transform.SetParent(gameObject.transform);
            if (type == 0)
            {
                g.transform.position = pos;
                if (!World.GreedMode)
                {
                    g.GetComponentInChildren<Image>().sprite = coin;
                    g.GetComponent<Text>().color = new Color(1, 0.8f, 0);
                }
                else
                {
                    g.GetComponentInChildren<Image>().sprite = coin;
                    g.GetComponentInChildren<Image>().color = new Color(1, 0, 0);
                    g.GetComponent<Text>().color = new Color(1, 0, 0);
                }

            }
            else if (type == 1)
            {
                g.transform.position = pos;
                g.GetComponentInChildren<Image>().sprite = stamina;
                g.GetComponentInChildren<Image>().color = new Color(0.5f, 0.5f, 1);
                g.GetComponent<Text>().color = new Color(0.5f, 0.5f, 1);
            }
            else
            {
                g.transform.position = pos;
                g.GetComponentInChildren<Image>().sprite = fire;
                g.GetComponentInChildren<Image>().color = new Color(1, 0.4f, 0);
                g.GetComponent<Text>().color = new Color(1, 0.4f, 0);
            }
            g.GetComponent<Text>().text = value[0] == '-' || value[0] == '�' ? value : "+" + value;
        }
        void FixedUpdate()
        {
            foreach (Transform notice in transform)
            {
                Color color = notice.GetComponentInChildren<Text>().color;
                notice.GetComponentInChildren<Text>().color = new Color(color.r, color.g, color.b, color.a - 0.02f);
                notice.GetComponentInChildren<Image>().color = new Color(color.r, color.g, color.b, color.a - 0.02f);
                if (color.a < 0.01f)
                    Destroy(notice.gameObject);
                Vector3 pos = notice.transform.position;
                notice.transform.position = new Vector3(pos.x, pos.y + 0.2f, pos.z);
            }
        }
    }
}
