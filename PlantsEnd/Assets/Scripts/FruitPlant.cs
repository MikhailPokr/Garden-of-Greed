using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Garden
{
    public class FruitPlant : Plant
    {
        public GameObject fruit;
        public int maxFruit;
        public int fruitCost;
        internal override void Create()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            garden = GameObject.Find("Garden").GetComponent<World>();
            stagesSprites = garden.stagesSprites.GetRange(0, Random.Range(1, garden.stagesSprites.Count+1));
            stagesSprites.Add(garden.DoneSprites[Random.Range(0, garden.DoneSprites.Count)]);
            fruit = garden.fruits[Random.Range(0, garden.fruits.Count)];
            stageTime = Random.Range(4f, 20f);
            maxFruit = Random.Range(stagesSprites.Count + 3, 20);
            maxStage = Random.Range(maxFruit + 2, maxFruit + 20);
            fruitCost = Random.Range(2, 10);
            breeds = Random.value > 0.9f;
            woodCost = Random.Range(-2, 10);
        }
        internal override void CreateBreed()
        {
            for (int i = 0; i < Random.Range(1, 3); i++)
            {
                GameObject p = Instantiate(gameObject);
                float x = transform.position.x + Random.Range(-0.4f, 0.4f); float y = transform.position.y + Random.Range(-0.4f, 0.4f);
                p.transform.position = new Vector2(x, y);
                float size = p.GetComponent<SpriteRenderer>().bounds.min.y;
                p.transform.position = new Vector3(x, y - 0.09f, size + +0.2f);
                foreach (Transform fruit in p.transform) Destroy(fruit.gameObject);
                FruitPlant pl = p.GetComponent<FruitPlant>();
                pl.spriteRenderer = p.GetComponent<SpriteRenderer>();
                pl.stage = 0;
                pl.stagesSprites = garden.stagesWildSprites.GetRange(0, stagesSprites.Count - 1);
                pl.spriteRenderer.sprite = pl.stagesSprites[0];
                pl.spriteRenderer.color = new Color(1, 1, 1);
                pl.stagesSprites.Add(stagesSprites[stagesSprites.Count - 1]);
                pl.stageTime = stageTime + Random.Range(0.5f, 5);
                pl.maxFruit = maxFruit + Random.Range(-2, 5);
                pl.maxStage = maxStage + Random.Range(-2, 5);
                pl.fruitCost = fruitCost + Random.Range(-1, 5);
                pl.breeds = false;
                pl.woodCost = woodCost + Random.Range(-1, 5);
                p.transform.parent = gameObject.transform.parent;
                pl.garden = garden;
            }
        }
        private void PlantDone()
        {
            float random = Random.value;
            int count = random > 0.9f ? 2 : (random < 0.5f ? 0 : 1);
            for (int i = 0; i < count; i++)
            {
                GameObject spawnFruit = Instantiate(fruit);
                spawnFruit.transform.position = new Vector3(transform.position.x + Random.Range(-0.2f, 0.2f), transform.position.y + Random.Range(0, 0.3f), transform.position.z - 1f);
                spawnFruit.transform.parent = gameObject.transform;
            }
        }
        internal override void SetNextStage()
        {
            if (stage < stagesSprites.Count || stage >= maxStage-1)
            {
                base.SetNextStage();
                return;
            }
            if (stage == stagesSprites.Count)
            {
                timerEnabled = true;
                timerStart = World.time;
                stage++;
            }
            else if (stage < maxFruit)
            {
                PlantDone();
                timerStart = World.time;
                stage++;
            }
            else if (stage < maxStage)
            {
                spriteRenderer.color = new Color(0.8f, 0.8f, 0.8f);
                timerStart = World.time;
                stage++;
            }
        }
        internal override void OnMouseUpAsButton()
        {
            if (World.Mode == 5)
            {
                GameObject.Find("Page").GetComponent<PageManager>().SwitchPage(14, gameObject);
            }
            else
                base.OnMouseUpAsButton();
        }
    }
}
