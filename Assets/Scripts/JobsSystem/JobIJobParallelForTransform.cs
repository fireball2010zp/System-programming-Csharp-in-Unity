/*
3*. Создайте задачу типа IJobForTransform, которая будет вращать указанные Transform 
вокруг своей оси с заданной скоростью.
*/

using UnityEngine;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine.UI;
using UnityEngine.Jobs;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;

public class JobIJobParallelForTransform : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Vector3 _direction;
    [SerializeField] private int _count;
    [SerializeField] private float _spawnRadius;
    private NativeArray<int> _angle;
    private TransformAccessArray _accessArray;
  
    private void Start()
    {
        _angle = new NativeArray<int>(_count, Allocator.Persistent);
        _accessArray = new TransformAccessArray(SpawnObj(_prefab, _count, _spawnRadius));
        for (int i = 0; i < _count; i++)
        {
            _angle[i] = Random.Range(0, 360);
        }
    }
    private void OnDestroy()
    {
        if (_accessArray.isCreated)
        { 
            _accessArray.Dispose();
            _angle.Dispose();
        }
    }

    private void Update()
    {
        JobTransform myJobParTransform = new JobTransform()
        {
            direction = _direction,
            deltaTime = Time.deltaTime,
            angles = _angle
        };

        JobHandle jobHandle = myJobParTransform.Schedule(_accessArray);
        jobHandle.Complete();
    }

    Transform[] SpawnObj(GameObject prefab, int count, float spawnRadius)
    {
        Transform[] objects = new Transform[count];
        for (int i = 0; i < count; i++)
        {
            objects[i] = Instantiate(prefab).transform;
            objects[i].position = Random.insideUnitSphere * spawnRadius;
        }
        return objects;
    }
}

[BurstCompile]
public struct JobTransform : IJobParallelForTransform
{
    public Vector3 direction;
    public float deltaTime;
    public NativeArray<int> angles;

    public void Execute(int index, TransformAccess transform)
    {
        transform.position += direction * deltaTime;
        transform.localRotation = Quaternion.AngleAxis(angles[index], Vector3.up);
        angles[index] = angles[index] == 180 ? 0 : angles[index]+1;
    }

}