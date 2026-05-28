using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GardenOld
{
    public class Grass : MonoBehaviour
    {
        public Sprite Sprite;
        public int timer;
        public int cost;
        public void Update()
        {
            if (World.time > timer)
                Destroy(gameObject);
        }
        private void OnMouseUpAsButton()
        {
            cost = cost == 0 ? 0 : cost+Shop.BonusGrass;
            World.money += cost;
            Destroy(gameObject);
            GameObject.Find("Notices").GetComponent<Notice>().Show(0, cost.ToString());
            GameObject.Find("Page").GetComponent<PageManager>().ReturnDefault();
        }
        private void OnMouseEnter()
        {
                GameObject.Find("Page").GetComponent<PageManager>().SwitchPage(cost == 0 ? 6 : 7, gameObject);
        }
        private void OnMouseExit()
        {
            GameObject.Find("Page").GetComponent<PageManager>().ReturnDefault();
        }
        public void Create()
        {
            float x = Random.Range(-3.4f, 1.2f); float y = Random.Range(-1.9f, 1.9f);
            transform.position = new Vector2(x, y);
            float size = GetComponent<SpriteRenderer>().bounds.min.y;
            transform.position = new Vector3(x, y, size);
            if (World.GreedMode)
            {
                GetComponent<SpriteRenderer>().color = new Color(1, 1 + (World.stamina > -16 ? 0.05f * World.stamina : -0.8f), 1 + (World.stamina > -16 ? 0.05f * World.stamina : -0.8f));
            }
            GetComponent<SpriteRenderer>().sprite = Sprite;
            timer = (int)World.time + 30;
        }
    }

}
