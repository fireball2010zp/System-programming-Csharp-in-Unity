/*
Задание 3 (дополнительное).
Реализовать задачу WhatTaskFasterAsync, которая будет принимать в качестве параметров CancellationToken, 
а также две задачи в виде переменных типа Task. Задача должна ожидать выполнения хотя бы одной из задач, 
останавливать другую и возвращать результат. Если первая задача выполнена первой, вернуть true, если вторая — false. 
Если сработал CancellationToken, также вернуть false. Проверить работоспособность с помощью задач из Задания 2.
*/

using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace WhatTaskFaster
{
    public class Unit : MonoBehaviour
    {
        private Task<bool> Task1;
        private Task<bool> Task2;

        void Start()
        {
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken cancelToken = cancelTokenSource.Token;

            Task<bool> task = Task.Run(() => WhatTaskFasterAsync(cancelToken, Task1, Task2));
            
            cancelTokenSource.Dispose();
        }

        public async Task<bool> WhatTaskFasterAsync(CancellationToken cancelToken, Task<bool> task1, Task<bool> task2)
        {
            using (CancellationTokenSource linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancelToken))
            {
                CancellationToken linkedCt = linkedCts.Token;

                task1 = Task1Async(cancelToken);
                task2 = Task2Async(cancelToken);

                Task<bool> finishedTask = await Task.WhenAny(task1, task2);
                bool result = (finishedTask == task1 && finishedTask.Result == true);

                linkedCts.Cancel();

                Debug.Log(result);
                return result;
            }
        }

        async Task<bool> Task1Async(CancellationToken cancelToken)
        {
            var time = 1000;
            if (cancelToken.IsCancellationRequested)
            {
                Debug.Log("The operation was interrupted by a token!");
                return false;
            }
            await Task.Delay(time);
            Debug.Log($"Local control message: Task1 is finished after " + time + " milliseconds!");
            return true;
        }

        async Task<bool> Task2Async(CancellationToken cancelToken)
        {
            var frames = 0;
            while (frames < 60)
            {
                if (cancelToken.IsCancellationRequested)
                {
                    Debug.Log("The operation was interrupted by a token!");
                    return false;
                }
                frames++;
                await Task.Yield();
            }
            Debug.Log($"Local control message: Task2 is finished after " + frames + " frames!");
            return false;
        }
    }
}