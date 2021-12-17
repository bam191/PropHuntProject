using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UNET;
using UnityEngine;

public class ConnectionController : MonoBehaviour
{
    private UNetTransport _unetTransport;
    private NetworkManager _networkManager;

    private void Awake()
    {
        _networkManager = NetworkManager.Singleton;
        _unetTransport = _networkManager?.GetComponent<UNetTransport>();
    }

    public void ConnectToServer(string address, int port)
    {
        _unetTransport.ConnectAddress = address;
        _unetTransport.ConnectPort = port;
        _unetTransport.ServerListenPort = port;

        _networkManager.StartClient();
    }

    public void HostServer(int port)
    {
        _unetTransport.ConnectPort = port;
        _unetTransport.ServerListenPort = port;
    }
}
