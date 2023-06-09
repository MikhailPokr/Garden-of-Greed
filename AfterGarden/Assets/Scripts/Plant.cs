using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Garden
{
    public class Plant : MonoBehaviour
    {
        [SerializeField] internal List<Sprite> stagesSprites;
        [SerializeField] internal List<Sprite> stagesWildSprites;

        internal int stage = 0;

        [SerializeField] internal float stageTime;
        [SerializeField] internal int maxStage;
        [SerializeField] internal bool breeds;
        [SerializeField] internal int woodCost;

        internal bool timerEnabled = true;
        internal float timerStart;

        internal SpriteRenderer spriteRenderer;

        internal World garden;

        internal void Start()
        {
            SetNextStage();
        }
        internal void Update()
        {
            if (timerEnabled)
            {
                if (World.time > timerStart+stageTime)
                {
                    SetNextStage();
                }
            }
        }
        internal virtual void Create()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            garden = GameObject.Find("Garden").GetComponent<World>();
            stagesSprites = garden.stagesSprites.GetRange(0, Random.Range(1, garden.stagesSprites.Count+1));
            stagesSprites.Add(garden.DoneSprites[Random.Range(0, garden.DoneSprites.Count)]);
            stageTime = Random.Range(4f, 20f);
            maxStage = Random.Range(stagesSprites.Count + 4, 25);
            breeds = Random.value > 0.8f;
            woodCost = Random.Range(-2, 10);
            woodCost = woodCost < 0 ? -Mathf.RoundToInt(woodCost * Shop.BonusWood) - woodCost : Mathf.RoundToInt(woodCost * Shop.BonusWood);
            
        }
        internal virtual void CreateBreed()
        {
            for (int i = 0; i < Random.Range(1, 3); i++)
            {
                GameObject p = Instantiate(gameObject);
                float x = transform.position.x + Random.Range(-0.4f, 0.4f); float y = transform.position.y + Random.Range(-0.4f, 0.4f);
                p.transform.position = new Vector2(x, y);
                float size = p.GetComponent<SpriteRenderer>().bounds.min.y;
                p.transform.position = new Vector3(x, y - 0.09f, size + 0.2f);
                Plant pl = p.GetComponent<Plant>();
                pl.spriteRenderer = p.GetComponent<SpriteRenderer>();
                pl.stage = 0;
                pl.stagesSprites = garden.stagesWildSprites.GetRange(0, stagesSprites.Count - 1);
                pl.spriteRenderer.sprite = pl.stagesSprites[0];
                pl.stagesSprites.Add(stagesSprites[stagesSprites.Count - 1]);
                pl.stageTime = stageTime + Random.Range(0.5f, 5);
                pl.maxStage = maxStage + Random.Range(-2, 5);
                pl.breeds = Random.value > 0.9f;
                pl.woodCost = woodCost + Random.Range(-1, 5);
                pl.woodCost = pl.woodCost < 0 ? -Mathf.RoundToInt(pl.woodCost * ((Shop.BonusWood - 1) / 2 + 1)) - pl.woodCost : Mathf.RoundToInt(pl.woodCost * ((Shop.BonusWood - 1) / 2 + 1));
                p.transform.parent = gameObject.transform.parent;
                pl.garden = garden;
            }
        }
        internal virtual void SetNextStage()
        {
            if (stage < stagesSprites.Count - 1)
            {
                spriteRenderer.sprite = stagesSprites[stage];
                stage++;
                timerStart = World.time;
            }
            else if (stage == stagesSprites.Count - 1)
            {
                spriteRenderer.sprite = stagesSprites[stage];
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.2f, spriteRenderer.bounds.min.y + 0.2f);
                stage++;
                timerStart = World.time;
            }
            else if (stage == maxStage - 1)
            {
                if (breeds)
                    CreateBreed();
                stage++;
                timerStart = World.time;
            }
            else if (stage == maxStage)
            {
                foreach (Transform fruit in transform) Destroy(fruit.gameObject);
                timerEnabled = false;
                woodCost = -3;
                spriteRenderer.sprite = GetComponentInParent<World>().DieSprite;
            }
            else
            {
                stage++;
                timerStart = World.time;
            }
        }
        internal virtual void OnMouseUpAsButton()
        {
            if (World.Mode == 1 && World.stamina > 0)
            {
                Destroy(gameObject);
                World.stamina--;
                if (World.fire < 10)
                {
                    World.fire++;
                    World.EditFire();
                    GameObject.Find("Notices").GetComponent<Notice>().Show(2, (1).ToString());
                }
                World.EditStamina();
                GameObject.Find("Notices").GetComponent<Notice>().Show(1, (-1).ToString());
                GameObject.Find("Page").GetComponent<PageManager>().ReturnDefault();
            }
            else if (World.Mode == 2)
            {
                World.money += stage > stagesSprites.Count-1 ? woodCost : -1;
                if (stage == maxStage && World.fire < 10)
                {
                    World.fire++;
                    World.EditFire();
                    GameObject.Find("Notices").GetComponent<Notice>().Show(2, 1.ToString());
                }
                Destroy(gameObject);
                GameObject.Find("Notices").GetComponent<Notice>().Show(0, (stage > stagesSprites.Count -1 ? woodCost : -1).ToString());
                GameObject.Find("Page").GetComponent<PageManager>().ReturnDefault();
            }
            else if (World.Mode == 4 && World.money >= 80 && !breeds && timerEnabled)
            {
                breeds = true;
                World.money -= 80;
                GameObject.Find("Notices").GetComponent<Notice>().Show(0, (-80).ToString());
            }
            else if (World.Mode == 5)
            {
                GameObject.Find("Page").GetComponent<PageManager>().SwitchPage(13, gameObject);
            }
        }
        internal virtual void OnMouseEnter()
        {
            if (timerEnabled)
                GameObject.Find("Page").GetComponent<PageManager>().SwitchPage(8,gameObject);
            else
                GameObject.Find("Page").GetComponent<PageManager>().SwitchPage(11, gameObject);
        }
        private void OnMouseExit()
        {
            GameObject.Find("Page").GetComponent<PageManager>().ReturnDefault();
        }
    }
}
