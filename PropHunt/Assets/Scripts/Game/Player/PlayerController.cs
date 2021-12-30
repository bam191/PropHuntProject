using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private GameObject _localPlayer;
    [SerializeField] private GameObject _serverPlayer;

    private PlayerModelController _playerModelController;
    private PlayerAnimationController _playerAnimationController;
    
    public NetworkVariable<bool> _requestCrouch;
    public NetworkVariable<Vector3> _requestedPosition;
    public NetworkVariable<Vector3> _requestedRotation;
    
    public NetworkVariable<float> _health = new NetworkVariable<float>(100);
    public NetworkVariable<int> _kills = new NetworkVariable<int>(0);
    public NetworkVariable<int> _deaths = new NetworkVariable<int>(0);

    public NetworkVariable<eTeam> team;

    private void Awake()
    {
        _playerModelController = GetComponentInChildren<PlayerModelController>();
        _playerAnimationController = GetComponentInChildren<PlayerAnimationController>();
    }

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

        _playerModelController.SetTeam(team);
    }

    public void SetVelocity(Vector3 velocity)
    {
        _playerAnimationController.SetVelocity(velocity);
    }
    
    #region  RPCs

    [ServerRpc]
    public void MovementServerRpc(PlayerMovementInputs input)
    {
        _requestCrouch.Value = input.requestCrouch;
        _requestedPosition.Value = input.requestedPosition;
        _requestedRotation.Value = input.requestedRotation;
    }

    [ServerRpc]
    public void TakeDamageServerRpc(ulong damageDealer, float damage)
    {
        if (_health.Value > 0)
        {
            _health.Value -= damage;
            if (_health.Value <= 0)
            {
                _deaths.Value += 1;
                //dead;
            }
        }
    }

    [ServerRpc]
    public void TakeSelfDamageServerRpc(float damage)
    {
        if (_health.Value > 0)
        {
            _health.Value -= damage;
            if (_health.Value <= 0)
            {
                _deaths.Value += 1;
                //dead
            }
        }
    }

    #endregion
}
