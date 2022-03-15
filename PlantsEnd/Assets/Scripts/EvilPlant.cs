using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Garden
{
    public class EvilPlant : FruitPlant
    {
        internal override void SetNextStage()
        {
            base.SetNextStage();
            spriteRenderer.color = new Color(1, 0.7f, 0.7f);
        }
        internal override void Create()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            garden = GameObject.Find("Garden").GetComponent<World>();
            stagesSprites = garden.stagesSprites.GetRange(0, Random.Range(1, garden.stagesSprites.Count+1));
            stagesSprites.Add(garden.DoneSprites[Random.Range(0, garden.DoneSprites.Count)]);
            spriteRenderer.color = new Color(1, 0.7f, 0.7f);
            fruit = garden.fruits[Random.Range(0, garden.fruits.Count)];
            stageTime = Random.Range(4f, 12f);
            maxFruit = Random.Range(stagesSprites.Count + 3, 15);
            maxStage = Random.Range(maxFruit + 2, maxFruit + 5);
            fruitCost = Random.Range(-5, 0);
            breeds = Random.value > 0.5f;
            woodCost = Random.Range(-10, 0);
        }
        internal override void CreateBreed()
        {
            for (int i = 0; i < Random.Range(1, 4); i++)
            {
                GameObject p = Instantiate(gameObject);
                float x = transform.position.x + Random.Range(-0.4f, 0.4f); float y = transform.position.y + Random.Range(-0.4f, 0.4f);
                p.transform.position = new Vector2(x, y);
                float size = p.GetComponent<SpriteRenderer>().bounds.min.y;
                p.transform.position = new Vector3(x, y - 0.09f, size + 0.2f);
                foreach (Transform fruit in p.transform) Destroy(fruit.gameObject);
                FruitPlant pl = p.GetComponent<FruitPlant>();
                pl.spriteRenderer = p.GetComponent<SpriteRenderer>();
                pl.stage = 0;
                pl.stagesSprites = garden.stagesWildSprites.GetRange(0, stagesSprites.Count - 1);
                pl.spriteRenderer.sprite = pl.stagesSprites[0];
                pl.spriteRenderer.color = new Color(1, 1, 1);
                pl.stagesSprites.Add(stagesSprites[stagesSprites.Count - 1]);
                pl.stageTime = stageTime + Random.Range(-2f, 1f);
                if (pl.stageTime < 1)
                {
                    pl.stageTime = 1;
                }
                pl.maxFruit = maxFruit + Random.Range(-2, 5);
                pl.maxStage = maxStage + Random.Range(-2, 5);
                pl.fruitCost = fruitCost + Random.Range(-2, 4);
                pl.breeds = Random.value > 0.7f;
                pl.woodCost = woodCost + Random.Range(-2, 4);
                p.transform.parent = gameObject.transform.parent;
                pl.garden = garden;
            }
        }
        internal override void OnMouseEnter()
        {
            if (timerEnabled)
                GameObject.Find("Page").GetComponent<PageManager>().SwitchPage(10, gameObject);
            else
                GameObject.Find("Page").GetComponent<PageManager>().SwitchPage(12, gameObject);
        }
        internal override void OnMouseUpAsButton()
        {
            if (World.Mode == 5)
            {
                GameObject.Find("Page").GetComponent<PageManager>().SwitchPage(15, gameObject);
            }
            else
                base.OnMouseUpAsButton();
        }
    }
}
