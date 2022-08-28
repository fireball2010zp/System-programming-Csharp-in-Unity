/* Задание 1. Применить корутины.
Дано:
1. Класс Unit, у которого есть переменная health, отвечающая за текущее количество жизней.
2. Метод RecieveHealing().
Задача: реализовать корутину, которая будет вызываться из метода RecieveHealing, 
чтобы юнит получал исцеление 5 жизней каждые полсекунды в течение 3 секунд или до тех пор, 
пока количество жизней не станет равным 100. 
На юнит не может действовать более одного эффекта исцеления одновременно.
*/

using System.Collections;
using UnityEngine;

namespace Coroutine
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] private int health;
        private float time;

        void Start()
        {
            time = 0f;
            Debug.Log($"Unit health = " + health + "; Time healing (s) = " + time);
            ReceiveHealing();
        }

        public void ReceiveHealing()
        {
            StartCoroutine(Healing());
        }

        IEnumerator Healing()
        {
            while (health < 100 && time < 3f)
            {
                yield return new WaitForSeconds(0.5f);
                health += 5;
                time += 0.5f;
                Debug.Log($"Unit health = " + health + "; Time healing (s) = " + time);
            }
        }
    }
}



