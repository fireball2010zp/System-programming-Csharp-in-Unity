using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    [SerializeField]
    private GameObject playerPrefab;
    private GameObject playerCharacter;

    public void SpawnCharacter() // создаёт аватар нового игрока на клиенте, добавляя его копию на сервер и другим подключенными клиентам
    {
        if (!isServer)
        {
            return;
        }
        playerCharacter = Instantiate(playerPrefab);

        NetworkServer.SpawnWithClientAuthority(playerCharacter, connectionToClient);
        // метод SpawnWithClientAuthority() связывает созданный аватар игрока (при добавлении в серверную сцену) с клиентом-владельцем этого игрока
    }

    void Start()
    {
        SpawnCharacter();
    }
}
