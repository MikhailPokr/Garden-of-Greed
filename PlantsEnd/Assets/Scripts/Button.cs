using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Garden
{
    public class Button : MonoBehaviour
    {
        private int cheatClick = 0;
        private static bool InfoActive = false;
        public Texture2D cursorTexture;
        public CursorMode cursorMode = CursorMode.Auto;
        public Vector2 hotSpot = Vector2.zero;
        public int gameCursorMode;
        public void Click()
        {
            if (name == "Info")
            {
                if (!InfoActive)
                {
                    if (World.money < 300)
                        return;
                    else
                    {
                        World.money -= 300;
                        InfoActive = true;
                        transform.parent.GetComponent<Image>().color = new Color(0.9716981f, 0.8290372f, 0.5912691f);
                        GameObject.Find("Info Priñe").SetActive(false);
                    }
                }
            }
            if (name == "Leave")
            {
                if (World.money >= 2000)
                {
                    if (World.GreedMode)
                        SceneManager.LoadScene(2);
                    else
                        SceneManager.LoadScene(1);
                }
            }
            if (name == "Exit")
            {
                Application.Quit();
                print("Exit");
            }
            if (name == "Restart")
            {
                World.money = 50;
                World.stamina = 10;
                World.fire = 10;
                World.GreedMode = false;
                World.Mode = 0;
                World.time = 0;
                Cursor.SetCursor(null, hotSpot, cursorMode);
                SceneManager.LoadScene(0);
            }
            if (name == "Help")
            {
                Time.timeScale = 0;
                GameObject t = GameObject.Find("Page");
                foreach (Transform page in t.transform)
                {
                    if (!page.Equals(t.transform))
                        page.gameObject.SetActive(false);
                }
                t.GetComponent<PageManager>().PageVariations[17].SetActive(true);
            }
            if (gameCursorMode != -1)
            {
                Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
                World.Mode = gameCursorMode;
                GameObject.Find("Page").GetComponent<PageManager>().ReturnDefault();
            }
        }
        public void Cheat()
        {
            cheatClick++;
            if (cheatClick == 5)
            {
                World.money += 500;
                GameObject.Find("Notices").GetComponent<Notice>().Show(0, 500.ToString());
            }
        }
    }
}

