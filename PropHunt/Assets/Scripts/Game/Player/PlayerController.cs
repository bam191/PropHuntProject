using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private GameObject _localPlayer;
    [SerializeField] private GameObject _serverPlayer;
    
    public NetworkVariable<bool> requestCrouch;
    public NetworkVariable<Vector3> requestedPosition;
    public NetworkVariable<Vector3> requestedRotation;
    
    public NetworkVariable<int> health = new NetworkVariable<int>(100);
    public NetworkVariable<int> kills = new NetworkVariable<int>(0);
    public NetworkVariable<int> deaths = new NetworkVariable<int>(0);

    public NetworkVariable<eTeam> team;
    private void Start()
    {
        GameController.Instance.RegisterPlayer(this);

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

    public void SetTeam(eTeam team)
    {
        this.team.Value = team;
    }
    
    #region  RPCs

    [ServerRpc]
    public void MovementServerRpc(PlayerMovementInputs input)
    {
        requestCrouch.Value = input.requestCrouch;
        requestedPosition.Value = input.requestedPosition;
        requestedRotation.Value = input.requestedRotation;
    }

    #endregion
}
