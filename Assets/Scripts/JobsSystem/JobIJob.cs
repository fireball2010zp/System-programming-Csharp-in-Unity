/*
1. —оздайте задачу типа IJob, котора€ принимает данные в формате NativeArray<int> 
и в результате выполнени€ все значени€ более дес€ти делает равными нулю.
¬ызовите выполнение этой задачи из внешнего метода и выведите в консоль результат.
 */

using UnityEngine;
using Unity.Jobs;
using UnityEngine.Jobs;
using Unity.Collections;

public class JobIJob : MonoBehaviour
{
    NativeArray<int> intArray;

    void Start()
    {
        More10Zero(15, 9, 3, 25, 67);
    }
    
    private void More10Zero(int a, int b, int c, int d, int e)
    {
        intArray = new NativeArray<int>(new int[] { a, b, c, d, e }, Allocator.Persistent);

        ZeroJob myJob = new ZeroJob()
        {
            intArr = intArray
        };
        JobHandle jobHandle = myJob.Schedule();
        jobHandle.Complete();

        for (int i = 0; i < intArray.Length; i++)
            Debug.Log(intArray[i]);

        intArray.Dispose();
    }

    public struct ZeroJob : IJob
    {
        public NativeArray<int> intArr;

        public void Execute()
        {
            for (int i = 0; i < intArr.Length; i++)
            {
                if (intArr[i] > 10)
                    intArr[i] = 0;
            }
        }
    }
}
