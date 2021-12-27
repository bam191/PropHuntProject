using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UNET;
using UnityEngine;

public class LobbyController : Singleton<LobbyController>
{
    private NetworkManager _networkManager;
    private UNetTransport _unetTransport;
    private LoadingController _loadingController;
    private NetworkConfig _networkConfig;

    private ClientData _clientData;
    private LobbyData _lobbyData;

    private bool _isHosting;

    public override void Initialize()
    {
        _networkManager = NetworkManager.Singleton;
        _unetTransport = _networkManager?.GetComponent<UNetTransport>();
        _loadingController = LoadingController.Instance;
        _networkConfig = _networkManager.NetworkConfig;

        AddListeners();

        DontDestroyOnLoad(gameObject);
        base.Initialize();
    }

    private void AddListeners()
    {
        _networkManager.OnServerStarted += OnServerStarted;
        _networkManager.OnClientConnectedCallback += OnClientConnected;
        _networkManager.OnClientDisconnectCallback += OnClientDisconnect;
    }

    public void StartHost()
    {
        if (_isHosting) return;

        _isHosting = true;
        _networkManager.StartHost();
        LoadingController.Instance.LoadGameScene();
    }

    public void StartClient()
    {
        _networkConfig.ConnectionData = ClientData.GetBytes(_clientData);
        _networkManager.StartClient();
        LoadingController.Instance.LoadGameScene();
    }

    public void Disconnect()
    {
        _isHosting = false;

        _networkManager.Shutdown();
    }

    private void OnServerStarted()
    {
        Debug.LogError("Server started");
    }

    private void OnClientConnected(ulong clientId)
    {
        Debug.LogError("Client connected id " + clientId);
    }

    private void OnClientDisconnect(ulong clientId)
    {
        Debug.LogError("Client disconnected id " + clientId);
    }
}