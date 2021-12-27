using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private GameObject _localPlayer;
    [SerializeField] private GameObject _serverPlayer;
    
    private void Start()
    {
        if (IsLocalPlayer)
        {
            _localPlayer.SetActive(true);
            _serverPlayer.SetActive(false);
        }
        else
        {
            _localPlayer.SetActive(false);
            _serverPlayer.SetActive(true);
        }
    }
}
