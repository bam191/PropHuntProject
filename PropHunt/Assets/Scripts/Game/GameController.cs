using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController _instance;
    public static GameController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameController>(true);
            }

            return _instance;
        }
    }

    private NetworkManager _networkManager;

    private void Awake()
    {
        _networkManager = NetworkManager.Singleton;
    }
}
