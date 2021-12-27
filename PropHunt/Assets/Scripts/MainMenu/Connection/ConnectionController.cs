using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UNET;
using UnityEngine;

public class ConnectionController : MonoBehaviour
{
    private UNetTransport _unetTransport;
    private NetworkManager _networkManager;

    private int _defaultPort = 7777;

    private void Awake()
    {
        _networkManager = NetworkManager.Singleton;
        if (_networkManager == null)
        {
            Debug.LogError("Network manager does not exist. Make sure to load from the 'Startup' scene");
        }
        _unetTransport = _networkManager?.GetComponent<UNetTransport>();
    }

    public void ConnectToServer(string address, string username)
    {
        EnsureNetworkManagerExists();
        _unetTransport.ConnectAddress = address;
        _unetTransport.ConnectPort = _defaultPort;
        _unetTransport.ServerListenPort = _defaultPort;

        LobbyController.Instance.SetClientData(username);
        LobbyController.Instance.StartClient();
    }

    public void HostServer(string username)
    {
        EnsureNetworkManagerExists();
        _unetTransport.ConnectPort = _defaultPort;
        _unetTransport.ServerListenPort = _defaultPort;

        LobbyController.Instance.SetClientData(username);
        LobbyController.Instance.StartHost();        
    }

    private void EnsureNetworkManagerExists()
    {
        _networkManager = NetworkManager.Singleton;
        _unetTransport = _networkManager.GetComponent<UNetTransport>();
    }
}
