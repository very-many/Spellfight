using Mirror;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOrchestrator : NetworkBehaviour
{
    public enum GameState
    {
        Initial,
        Game,
        Upgrade
    }

    public static GameOrchestrator Instance { get; private set; }


    [Header("State")]
    [SerializeField]
    private GameState gameState = GameState.Initial;

    [Header("Scenes")]
    [SerializeField] private List<Object> GameScenes;
    [SerializeField] private Object UpgradeScene;


    [SyncVar(hook = nameof(OnReadyPlayersChanged))] public List<PlayerObjectController> readyPlayers;

    private CustomNetworkManager manager;

    private CustomNetworkManager Manager
    {
        get
        {
            if (manager != null)
                return manager;

            return manager = NetworkManager.singleton as CustomNetworkManager;
        }
    }

    private List<PlayerObjectController> Players
    {
        get
        {
            if (Manager == null)
                return new List<PlayerObjectController>();
            return Manager.GamePlayers;
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void NextGameState()
    {
        ShouldSwitchGameState();
    }

    void ShouldSwitchGameState()
    {
        if (!isServer)
            return;
        switch (gameState)
        {
            case GameState.Initial:
                EnterGameState();
                break;
            case GameState.Game:
                if (IsGameOver())
                {
                    EnterUpgradeState();
                }
                break;
            case GameState.Upgrade:
                if (IsEveryoneReady())
                {
                    EnterGameState();
                }
                break;
        }
    }

    bool IsGameOver()
    {
        bool atLeastOneDead = readyPlayers.Count > 0;
        bool lessThanOneAlive = Players.Count - readyPlayers.Count <= 1;
        return atLeastOneDead && lessThanOneAlive; //filter out players that left the game
    }

    bool IsGameOverForPlayer(PlayerObjectController player)
    {
        return readyPlayers.Count < Players.Count && !readyPlayers.Contains(player);
    }

    bool IsEveryoneReady()
    {
        return readyPlayers.Count == Players.Count;
    }

    void OnReadyPlayersChanged(List<PlayerObjectController> oldPlayers, List<PlayerObjectController> newPlayers)
    {
        ShouldSwitchGameState();
    }

    void EnterUpgradeState()
    {
        readyPlayers.Clear();
        Manager.ServerChangeScene(UpgradeScene.name);
        //player.UI.OpenUpgradeScreen();
        gameState = GameState.Upgrade;
        WirePlayerUi();
    }

    void EnterGameState()
    {
        readyPlayers.Clear();
        string randomScene = GameScenes[Random.Range(0, GameScenes.Count)].name;
        Debug.Log("Entering Game State and switch Scene to " + randomScene);
        Manager.ServerChangeScene(randomScene);
        gameState = GameState.Game;
        WirePlayerUi();
    }

    void WirePlayerUi()
    {
        GameObject LocalPlayerObject = GameObject.Find("LocalGamePlayer");
        if (LocalPlayerObject == null) return;
        PlayerMenuCaller PlayerMenuCaller = LocalPlayerObject.GetComponent<PlayerMenuCaller>();
        if (PlayerMenuCaller == null) return;

        PlayerMenuCaller.WireExternalUi();
    }
}
