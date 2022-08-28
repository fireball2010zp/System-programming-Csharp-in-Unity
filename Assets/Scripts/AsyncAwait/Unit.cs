/* ������� 2.��������� async / await.
����������� ��� ������: Task1 � Task2. � �������� ���������� ������ ������ ��������� CancellationToken. 
������ ������ ������ ������� ���� �������, � ����� �������� � ������� ��������� � ���� ����������. 
������ ������ ������ ������� 60 ������, � ����� � �������� ��������� � �������.
*/

using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace AsyncAwait
{
    public class Unit : MonoBehaviour
    {
        async void Start()
        {
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken cancelToken = cancelTokenSource.Token;

            Task<int> task1 = Task1Async(cancelToken);
            Task<int> task2 = Task2Async(cancelToken);
            await Task.WhenAll(task1, task2);
            Debug.Log("All tasks are finished!");

            cancelTokenSource.Cancel();
            cancelTokenSource.Dispose();
        }

        async Task<int> Task1Async(CancellationToken cancelToken)
        {
            var time = 1000;
            if (cancelToken.IsCancellationRequested)
            {
                Debug.Log("The operation was interrupted by a token!");
                return time;
            }
            await Task.Delay(time);
            Debug.Log($"Local control message: Task1 is finished after " + time + " milliseconds!");
            return time;
        }

        async Task<int> Task2Async(CancellationToken cancelToken)
        {
            var frames = 0;
            while (frames < 60)
            {
                if (cancelToken.IsCancellationRequested)
                {
                    Debug.Log("The operation was interrupted by a token!");
                    return frames;
                }
                frames++;
                await Task.Yield();
            }
            Debug.Log($"Local control message: Task2 is finished after " + frames + " frames!");
            return frames;
        }
    }
}


