using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    private NetworkManager _networkManager;

    public override void Initialize()
    {
        _networkManager = NetworkManager.Singleton;
        base.Initialize();
    }
}
