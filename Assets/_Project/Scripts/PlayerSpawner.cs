using Fusion;
using UnityEngine;

public class PlayerSpawner: MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    private PlayerMovement _playerMovement;
    public void PlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (player == runner.LocalPlayer)
        {
            _playerMovement = runner.Spawn(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity).GetComponent<PlayerMovement>();
            ReferenceManager.Instane.playerMovement = _playerMovement;
        }
    }
}