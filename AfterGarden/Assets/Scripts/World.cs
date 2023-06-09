using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Garden
{
    public class World : MonoBehaviour
    {
        public static int stamina = 10;
        public static float time = 0;
        public static int money = 50;
        public static int fire = 10;

        public Text moneyText;
        public Text timerText;

        public List<GameObject> fruits;

        public List<Sprite> stagesSprites;
        public List<Sprite> stagesWildSprites;
        public List<Sprite> DoneSprites;

        public Sprite DieSprite;

        public List<Sprite> grassSprites;

        public GameObject plant;
        public GameObject grass;

        public static int Mode;
        public static bool GreedMode = false;

        private float firetimer;

        public void FixedUpdate()
        {
            moneyText.text = "x" + money;
            timerText.text = (int)time / 60 + ":" + (int)time % 60;
            time += Time.deltaTime;
            if (Random.value > 0.94f)
            {
                GameObject p = Instantiate(grass);
                p.transform.parent = transform;
                Grass pg = p.GetComponent<Grass>();
                if (Random.value > 0.9f)
                {
                    pg.Sprite = grassSprites[Random.Range(0, grassSprites.Count)];
                    pg.cost = 1;
                }
                pg.Create();
            }
            if (time > firetimer + 30)
            {
                fire--;
                if (fire != 0)
                    GameObject.Find("Notices").GetComponent<Notice>().Show(2, (-1).ToString(), new Vector3(1340, 930, 0));
                EditFire();
                firetimer = time;
            }
        }
        public void PlantTree(float x, float y)
        {
            GameObject p = Instantiate(plant);
            float random = Random.value;
            if (random > 0.5f)
                p.AddComponent<Plant>().Create();
            else if (random > (!GreedMode ? 0.05f : 0.2f))
                p.AddComponent<FruitPlant>().Create();
            else
                p.AddComponent<EvilPlant>().Create();
            p.transform.parent = gameObject.transform;
            p.transform.position = new Vector2(x, y);
            float size = p.GetComponent<SpriteRenderer>().bounds.min.y;
            p.transform.position = new Vector3(x, y, size);

        }
        private void OnMouseUpAsButton()
        {
            if (Mode == 3)
            {
                if (money < 10)
                    return;
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                PlantTree(mousePos.x+0.01f, mousePos.y-0.02f);
                money -= 10;
                GameObject.Find("Notices").GetComponent<Notice>().Show(0, (-10).ToString());
            }
        }
        public static void EditStamina()
        {
            GameObject StaminaIndicator = GameObject.Find("Stamina");
            Image[] Ss = StaminaIndicator.GetComponentsInChildren<Image>();
            if (stamina < 0)
            {
                GreedMode = true;
                foreach (Transform S in StaminaIndicator.transform)
                {
                    if (!S.transform.Equals(StaminaIndicator.transform))
                        S.GetComponent<Image>().color = new Color(1f, 0.2f, 0f);
                }
                Color color = new Color(0.15f,0.4f,0);
                GameObject.Find("Main Camera").GetComponent<Camera>().backgroundColor = new Color(stamina > -16 ? color.r - 0.02f * stamina : 0.47f, stamina > -16 ? color.g + 0.01f * stamina : 0.24f, color.b);
                if (stamina < -10)
                    GameObject.Find("Blood").GetComponent<Image>().color = new Color(1,1,1,(stamina+10)*-0.1f >= 0.6f ? 0.6f : (stamina + 10) * -0.1f);
                else 
                    GameObject.Find("Blood").GetComponent<Image>().color = new Color(1, 1, 1, 0);
                return;
            }
            GreedMode = false;
            foreach (Image S in Ss)
            {
                if (S.name.Length > 2)
                    continue;
                if (int.Parse($"{S.name[1]}") > stamina - 1)
                    S.color = new Color(0.2f, 0.2f, 0.2f);
                else
                    S.color = new Color(0.5f, 0.5f, 1);
            }
        }
        public static void EditFire()
        {
            GameObject FireIndicator = GameObject.Find("Fire");
            Image[] Ss = FireIndicator.GetComponentsInChildren<Image>();
            if (fire <= 0)
            {
                fire += 1;
                money -= 10;
                GameObject.Find("Notices").GetComponent<Notice>().Show(0, (-10).ToString(), new Vector3(1340,930,0));
            }
            foreach (Image S in Ss)
            {
                if (S.name.Length > 2)
                    continue;
                if (int.Parse($"{S.name[1]}") > fire - 1)
                    S.color = new Color(0.2f, 0.2f, 0.2f);
                else
                    S.color = new Color(1f, 0.4f, 0);
            }
        }
    }
}