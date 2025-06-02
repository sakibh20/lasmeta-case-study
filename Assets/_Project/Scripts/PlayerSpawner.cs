using Fusion;
using UnityEngine;

public class PlayerSpawner: MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private Marker markerPrefab1;
    [SerializeField] private Marker markerPrefab2;

    private PlayerMovement _playerMovement;
    private NetworkRunner _runner;
    private PlayerRef _player;
    private bool _receivedShowMarkerCommand;
    private bool _isHost;
    public void PlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        _runner = runner;
        _player = player;
        
        if (player == runner.LocalPlayer)
        {
            _playerMovement = _runner.Spawn(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity).GetComponent<PlayerMovement>();
            ReferenceManager.Instane.playerMovement = _playerMovement;

            if (_receivedShowMarkerCommand)
            {
                ShowMarker(_isHost);
            }
        }
    }

    public void ShowMarker(bool isHost)
    {
        _isHost = isHost;
        _receivedShowMarkerCommand = true;
        
        if(_runner == null) return;
        
        if (_isHost)
        {
            _runner.Spawn(markerPrefab1, ReferenceManager.Instane.markerPos1.position, Quaternion.identity).GetComponent<Marker>();
        }
        else
        {
            _runner.Spawn(markerPrefab2, ReferenceManager.Instane.markerPos2.position, Quaternion.identity).GetComponent<Marker>();
        }
    }
}