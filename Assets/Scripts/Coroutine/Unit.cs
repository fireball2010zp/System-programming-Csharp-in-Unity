/* ������� 1. ��������� ��������.
����:
1. ����� Unit, � �������� ���� ���������� health, ���������� �� ������� ���������� ������.
2. ����� RecieveHealing().
������: ����������� ��������, ������� ����� ���������� �� ������ RecieveHealing, 
����� ���� ������� ��������� 5 ������ ������ ���������� � ������� 3 ������ ��� �� ��� ���, 
���� ���������� ������ �� ������ ������ 100. 
�� ���� �� ����� ����������� ����� ������ ������� ��������� ������������.
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



