using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UNET;
using UnityEngine;
using static Unity.Netcode.NetworkManager;

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
        _networkConfig.ConnectionApproval = true;
        _networkManager.ConnectionApprovalCallback += ConnectionApproval;

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

        _clientData = new ClientData();
        _clientData.Username = "alexi";
        _networkConfig.ConnectionData = ClientData.GetBytes(_clientData);

        _networkManager.StartHost();        
    }

    public void StartClient()
    {
        _clientData = new ClientData();
        _clientData.Username = "alexi";

        _networkConfig.ConnectionData = ClientData.GetBytes(_clientData);

        _networkManager.StartClient();
    }

    public void ConnectionApproval(byte[] connectionData, ulong clientId, ConnectionApprovedDelegate connectionApprovedDelegate)
    {
        Debug.LogError(ClientData.FromBytes(connectionData).Username);

        bool createPlayerObject = true;
        bool approved = true;

        LoadingController.Instance.LoadGameScene();

        LoadingController.Instance.OnSceneLoadedCallback += () =>
        {
            connectionApprovedDelegate(createPlayerObject, null, approved, Vector3.zero, Quaternion.identity);
            LoadingController.Instance.OnSceneLoadedCallback = null;
        };
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
