using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Garden
{
    public class PageManager : MonoBehaviour
    {
        public GameObject[] PageVariations;
        //0 - Mode 0 Default
        //1 - Mode 1 Default
        //2 - Mode 2 Default
        //3 - Mode 3 Default
        //4 - Mode 4 Default
        //5 - Mode 5 Default
        //6 - Grass Description
        //7 - Super Grass Description 
        //8 - Tree Description 
        //9 - Fruit Description 
        //10 - Evil Tree Description 
        //11 - Dead Tree Description 
        //12 - Dead Evil Tree Description
        //13 - Tree Super Description
        //14 - FruitTree Super Description
        //15 - EvilTree Super Description
        //16 - Mode 0 Greed

        public void SwitchPage(int pageNum, GameObject obj)
        {
            foreach (Transform page in transform)
            {
                if (!page.Equals(this))
                    page.gameObject.SetActive(false);
            }
            PageVariations[pageNum].SetActive(true);
            switch (pageNum)
            {
                case 7:
                    PageVariations[pageNum].GetComponentInChildren<Image>().sprite = obj.GetComponent<SpriteRenderer>().sprite;
                    goto default;
                case 8:
                    goto case 7;
                case 9:
                    goto case 7;
                case 10:
                    goto case 7;
                case 13:
                    Plant p = obj.GetComponent<Plant>();
                    string text = 
                        $"Дерево\n\n" +
                        $"Текущая стадия: {p.stage}\n" +
                        $"Время на страдию: {p.stageTime}\n" +
                        $"Максимум стадий: {p.maxStage}\n" +
                        $"Цена продажи древесины: {(p.stage > p.stagesSprites.Count-1 ? p.woodCost : -1)}{(p.stage > p.stagesSprites.Count-1 ? " " : $"(позже: {p.woodCost})")}\n" +
                        $"Пускает ли побеги: {(p.breeds ? "да" : "нет")}\n\n\n\n\n\n" +
                        $"Игра на паузе, отведите мышку в сторону, чтобы продолжить";
                    PageVariations[pageNum].GetComponentInChildren<Text>().text = text;
                    Time.timeScale = 0;
                    goto case 7;
                case 14:
                    FruitPlant p2 = obj.GetComponent<FruitPlant>();
                    string text2 =
                        $"Фруктовое Дерево\n\n" +
                        $"Текущая стадия: {p2.stage}\n" +
                        $"Время на страдию: {p2.stageTime}\n" +
                        $"Максимум стадий: {p2.maxStage}\n" +
                        $"Цена продажи древесины: {(p2.stage > p2.stagesSprites.Count-1 ? p2.woodCost : -1)}{(p2.stage > p2.stagesSprites.Count-1 ? " " : $"(позже: {p2.woodCost})")}\n" +
                        $"Не будет давать плоды после стадии: {p2.maxFruit}\n" +
                        $"Стоимость фрукта: {p2.fruitCost}\n" +
                        $"Пускает ли побеги: {(p2.breeds ? "да" : "нет")}\n\n\n\n" +
                        $"Игра на паузе, отведите мышку в сторону, чтобы продолжить";
                    PageVariations[pageNum].GetComponentInChildren<Text>().text = text2;
                    Time.timeScale = 0;
                    goto case 7;
                case 15:
                    EvilPlant p3 = obj.GetComponent<EvilPlant>();
                    string text3 =
                        $"Злое Дерево\n\n" +
                        $"Текущая стадия: {p3.stage}\n" +
                        $"Время на страдию: {p3.stageTime}\n" +
                        $"Максимум стадий: {p3.maxStage}\n" +
                        $"Цена продажи древесины: {(p3.stage > p3.stagesSprites.Count-1 ? p3.woodCost : -1)}{(p3.stage > p3.stagesSprites.Count-1 ? " " : $"(позже: {p3.woodCost})")}\n" +
                        $"Не будет давать плоды после стадии: {p3.maxFruit}\n" +
                        $"Стоимость фрукта: {p3.fruitCost}\n" +
                        $"Пускает ли побеги: {(p3.breeds ? "да" : "нет")}\n\n\n\n" +
                        $"Игра на паузе, отведите мышку в сторону, чтобы продолжить";
                    PageVariations[pageNum].GetComponentInChildren<Text>().text = text3;
                    Time.timeScale = 0;
                    goto case 7;
                default:
                    PageVariations[pageNum].SetActive(true);
                    break;
            }
        }
        public void ReturnDefault()
        {
            if (Time.timeScale == 0)
                Time.timeScale = 1;
                
            foreach (Transform page in transform)
            {
                if (!page.Equals(transform))
                    page.gameObject.SetActive(false);
            }
            if (World.Mode == 0 && World.GreedMode)
                PageVariations[16].SetActive(true);
            else
                PageVariations[World.Mode].SetActive(true);
        }
    }
}
