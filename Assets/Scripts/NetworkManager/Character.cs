using System;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(CharacterController))]
public abstract class Character : NetworkBehaviour // базовое поведение всех внутриигровых персонажей
{
    protected Action OnUpdateAction { get; set; }
    protected abstract FireAction fireAction { get; set; }

    [SyncVar] protected Vector3 serverPosition; // поле для каждого экземпляра объекта Character, чтобы синхронизировать положение объекта в сцене
    // атрибут SyncVar - для синхронизируемого поля (получения данных от сервера о состоянии текущего локального игрового объекта и аватара другого игрока в сцене,
    // владельцем которого не является текущий клиент)
    [SyncVar] protected Quaternion serverRotation;

    protected virtual void Initiate()
    {
        OnUpdateAction += Movement;
    }
    private void Update()
    {
        OnUpdate();
    }
    private void OnUpdate()
    {
        OnUpdateAction?.Invoke();
    }
    
    [Command] // атрибут используется для отправки синхронизируемого значения игрового объекта, владельцем которого является текущий клиент
    protected void CmdUpdatePosition(Vector3 position, Quaternion rotation) // метод отправки на сервер текущего положения нашего аватара
    {
        serverPosition = position;
        serverRotation = rotation;
    }

    public abstract void Movement();

    /*
    [Server]
    private void OnTriggerEnter(Collider other)
    {
        
    }*/
}
