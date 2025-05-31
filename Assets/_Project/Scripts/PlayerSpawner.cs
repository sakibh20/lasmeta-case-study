using Fusion;
using UnityEngine;

public class PlayerSpawner: MonoBehaviour
{
    [SerializeField] private NetworkEvents networkEvents;
    public GameObject playerPrefab;
    
    private void Start()
    {
        networkEvents.PlayerJoined.AddListener(PlayerJoined);
    }

    private void OnDestroy()
    {
        networkEvents.PlayerJoined.RemoveListener(PlayerJoined);
    }

    private void PlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        //Debug.Log("PlayerJoined");
        if (player == runner.LocalPlayer)
        {
            //Debug.Log("Runner.Spawn");
            runner.Spawn(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity);
        }
    }
}