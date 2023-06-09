using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Garden
{
    public static class Shop
    {
        public static bool ShopActive = false;
        public static int level = 0;
        public static float BonusWood = 1;
        public static float BonusFruit = 1;
        public static int BonusGrass = 0;
        public static int value = 10;
        public static bool BuyBonus(int type)
        {
            if (World.money < value)
                return false;
            switch (type)
            {
                case 0:
                    if (BonusWood < 3)
                        BonusWood += 0.2f;
                    else
                        return false;
                    break;
                case 1:
                    if (BonusFruit < 2)
                        BonusFruit += 0.1f;
                    else
                        return false;
                    break;
                case 2:
                    if (BonusGrass < 5)
                        BonusGrass++;
                    else
                        return false;
                    break;
            }
            World.money -= value;
            GameObject.Find("Notices").GetComponent<Notice>().Show(0, (-value).ToString());
            value = Mathf.RoundToInt(value*1.5f);
            level++;
            return true;
        }
        public static void Clear()
        {
           ShopActive = false;
           level = 0;
           BonusWood = 1;
           BonusFruit = 1;
           BonusGrass = 1;
           value = 10;
        }
    }
}
