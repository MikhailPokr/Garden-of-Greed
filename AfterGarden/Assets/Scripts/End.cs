using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Garden
{
    public class End : MonoBehaviour
    {
        void Start()
        {
            if (SceneManager.GetActiveScene().name == "BadEnd")
                GameObject.Find("Text").GetComponent<Text>().text =
                "Вы сделали это. Вам можно похлопать.\n\n" +
                "Только интересно, что за люди в капюшонах постоянно покупали у вас ядовитые фрукты? Впрочем какая разница, наверняка какие-нибудь ученые или врачи.\n\n" +
                "Вы нашли подходящий дом. Вам осталось только приехать туда и отдать свои деньги, чтобы осуществить свою мечту.\n" +
                "Вы посмотрели в зеркало. В существе перед вами с трудом угадывался человек. Вы все равно что ходячий труп. Продадут ли вам дом?\n" +
                $"А нужен ли он вам? Почему бы не устроить тут предприятие, и получать горы золота ежедневно. Вы посмотрели на свои деньги. {World.money} монет. Этого недостаточно. Нужно больше.\n\n" +
                 "С того дня вы продолжили приумножать свое багатсво, пока не умерли от токсинов фруктов.";
            else
                GameObject.Find("Text").GetComponent<Text>().text =
                "Поздравляем, вы выполнили свою цель!\n\n" +
                "Вы купили себе небольшой дом неподалеку, а сад поделили со своим приятелем, который отдает вам часть прибыли.\n" +
                "Эта глава вашей жизни закончена, теперь вы можете заняться чем-то другим.\n" +
                "Но это уже другая история.\n\n" +
                "Спасибо за игру.";
        }
        private void Update()
        {
            var p = GameObject.Find("Text").GetComponent<Text>().color;
            GameObject.Find("Text").GetComponent<Text>().color = new Color(p.r, p.g, p.b, p.a + 0.01f);
        }
    }
}
