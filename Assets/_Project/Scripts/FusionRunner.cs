using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FusionRunner : MonoBehaviour
{
    private NetworkRunner _runner;

    [ContextMenu("Connect")]
    private async void Connect()
    {
        _runner = GetComponent<NetworkRunner>();
        _runner.ProvideInput = true;
        
        var sceneInfo = new NetworkSceneInfo();
        
        if (TryGetSceneRef(out var sceneRef)) {
            if (sceneRef.IsValid) {
                sceneInfo.AddSceneRef(sceneRef, LoadSceneMode.Additive);
            }

            await _runner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.Shared,
                SessionName = "LasMetaPokerRoom",
                Scene = sceneInfo,
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            });
        }
    }
    
    private bool TryGetSceneRef(out SceneRef sceneRef) {
        var activeScene = SceneManager.GetActiveScene();
        if (activeScene.buildIndex < 0 || activeScene.buildIndex >= SceneManager.sceneCountInBuildSettings) {
            sceneRef = default;
            return false;
        } else {
            sceneRef = SceneRef.FromIndex(activeScene.buildIndex);
            return true;
        }
    }
}