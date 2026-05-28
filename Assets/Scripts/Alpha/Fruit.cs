using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GardenOld
{
    public class Fruit : MonoBehaviour
    {
        private void OnMouseUpAsButton()
        {
            EvilPlant EvilTree;
            bool ItsEvil = transform.parent.TryGetComponent(out EvilTree);
            Destroy(gameObject);
            if (World.stamina != 10 && !ItsEvil)
            {
                World.stamina++;
                World.EditStamina();
                GameObject.Find("Notices").GetComponent<Notice>().Show(1, 1.ToString());
                GameObject.Find("Page").GetComponent<PageManager>().ReturnDefault();
            }
            else
            {
                World.money += GetComponentInParent<FruitPlant>().fruitCost;
                GameObject.Find("Notices").GetComponent<Notice>().Show(0, GetComponentInParent<FruitPlant>().fruitCost.ToString());
                GameObject.Find("Page").GetComponent<PageManager>().ReturnDefault();
                if (ItsEvil)
                {
                    World.stamina--;
                    World.EditStamina();
                    if (!World.GreedMode)
                        GameObject.Find("Notices").GetComponent<Notice>().Show(1, (-1).ToString());
                    GameObject.Find("Page").GetComponent<PageManager>().ReturnDefault();
                }
                
            }
        }
        private void OnMouseEnter()
        {
            GameObject.Find("Page").GetComponent<PageManager>().SwitchPage(9, gameObject);
        }
        private void OnMouseExit()
        {
            GameObject.Find("Page").GetComponent<PageManager>().ReturnDefault();
        }
    }
}
