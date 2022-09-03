/*
2. Cоздайте задачу типа IJobParallelFor, которая будет принимать данные в виде двух контейнеров: 
Positions и Velocities — типа NativeArray<Vector3>. 
Также создайте массив FinalPositions типа NativeArray<Vector3>.
Сделайте так, чтобы в результате выполнения задачи в элементы массива FinalPositions были записаны
суммы соответствующих элементов массивов Positions и Velocities.
Вызовите выполнение созданной задачи из внешнего метода и выведите в консоль результат.
*/

using UnityEngine;
using Unity.Jobs;
using UnityEngine.Jobs;
using Unity.Collections;

public class JobIJobParallelFor : MonoBehaviour 
{
    NativeArray<Vector3> Positions;
    NativeArray<Vector3> Velocities;
    NativeArray<Vector3> FinalPositions;

    void Start()
    {
        ArraySums();
    }

    private void ArraySums()
    {
        Positions = new NativeArray<Vector3>
            (new Vector3[] 
            {   Vector3.down, 
                Vector3.up, 
                Vector3.left 
            }, Allocator.Persistent);
        
        Velocities = new NativeArray<Vector3>
            (new Vector3[] 
            {   Vector3.down, 
                Vector3.right, 
                Vector3.forward 
            }, Allocator.Persistent);

        FinalPositions = new NativeArray<Vector3>
            (new Vector3[]
            {   Vector3.zero,
                Vector3.zero,
                Vector3.zero
            }, Allocator.Persistent);

        FinalPos myJob = new FinalPos()
        {
            intArr1 = Positions,
            intArr2 = Velocities,
            intArr3 = FinalPositions
        };
        JobHandle jobHandle = myJob.Schedule(FinalPositions.Length,0);
        jobHandle.Complete();

        for (int i = 0; i < FinalPositions.Length; i++)
            Debug.Log(FinalPositions[i]);

        Positions.Dispose();
        Velocities.Dispose();
        FinalPositions.Dispose();
    }

    public struct FinalPos : IJobParallelFor
    {
        [ReadOnly]
        public NativeArray<Vector3> intArr1;
        [ReadOnly]
        public NativeArray<Vector3> intArr2;
        [WriteOnly]
        public NativeArray<Vector3> intArr3;

        public void Execute(int index)
        {
            for (int i = 0; i < intArr3.Length; i++)
            {
                intArr3[index] = intArr1[index] + intArr2[index];
            }
        }
    }
}
