using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    [SerializeField]
    private GameObject playerPrefab;
    private GameObject playerCharacter;

    public void SpawnCharacter() // ������ ������ ������ ������ �� �������, �������� ��� ����� �� ������ � ������ ������������� ��������
    {
        if (!isServer)
        {
            return;
        }
        playerCharacter = Instantiate(playerPrefab);

        NetworkServer.SpawnWithClientAuthority(playerCharacter, connectionToClient);
        // ����� SpawnWithClientAuthority() ��������� ��������� ������ ������ (��� ���������� � ��������� �����) � ��������-���������� ����� ������
    }

    void Start()
    {
        SpawnCharacter();
    }
}
