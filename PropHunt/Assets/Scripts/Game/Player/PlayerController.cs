using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private GameObject _localPlayer;
    [SerializeField] private GameObject _localFreeCam;
    [SerializeField] private GameObject _localProp;

    [SerializeField] private GameObject _serverPlayer;
    [SerializeField] private GameObject _serverFreeCam;
    [SerializeField] private GameObject _serverProp;

    private PlayerAnimationController _playerAnimationController;
    private PlayerCameraController _playerCameraController;
    private PlayerWeaponController _playerWeaponController;
    private NameplateController _nameplateController;

    public NetworkVariable<bool> _requestCrouch;
    public NetworkVariable<Vector3> _requestedPosition;
    public NetworkVariable<Vector3> _requestedRotation;
    
    public NetworkVariable<float> _health = new NetworkVariable<float>(100);
    public NetworkVariable<int> _kills = new NetworkVariable<int>(0);
    public NetworkVariable<int> _deaths = new NetworkVariable<int>(0);

    public NetworkVariable<eTeam> _team;

    private ClientData _clientData;
    public NetworkVariable<FixedString128Bytes> _userName;

    public eTeam Team
    {
        get
        {
            return _team.Value;
        }
    }

    private void Awake()
    {
        _playerAnimationController = GetComponentInChildren<PlayerAnimationController>();
        _playerCameraController = GetComponentInChildren<PlayerCameraController>();
        _playerWeaponController = GetComponentInChildren<PlayerWeaponController>();
        _nameplateController = GetComponentInChildren<NameplateController>();
    }

    private void Start()
    {
        GameController.Instance.RegisterPlayer(this);
        _clientData = LobbyController.Instance.GetClientData();
        _userName.OnValueChanged += UpdateUserName;
        _team.OnValueChanged += UpdateTeam;

        if (IsLocalPlayer)
        {
            _localPlayer.SetActive(true);
            _localFreeCam.SetActive(false);
            _localProp.SetActive(false);

            _serverPlayer.SetActive(false);
            _serverFreeCam.SetActive(false);
            _serverProp.SetActive(false);

            SetPlayerNameServerRPC((FixedString128Bytes)_clientData.Username);
        }
        else
        {
            _localPlayer.SetActive(false);
            _localFreeCam.SetActive(false);
            _localProp.SetActive(false);

            _serverPlayer.SetActive(true);
            _serverFreeCam.SetActive(false);
            _serverProp.SetActive(false);
        }
    }

    public void UpdateUserName(FixedString128Bytes previousValue, FixedString128Bytes newValue)
    {
        _nameplateController.SetName(newValue.ToString());
    }

    public void SetTeam(eTeam team)
    {
        _team.Value = team;
        _playerCameraController.SetTeam(team);
    }

    public void UpdateTeam(eTeam previousValue, eTeam newValue)
    {
        _playerCameraController.SetTeam(newValue);
    }

    public void SetVelocity(Vector3 velocity)
    {
        _playerAnimationController.SetVelocity(velocity);
    }

    public void AddRecoil(float recoil, float recoilMultiplier)
    {
        _playerCameraController.AddRecoil(recoil, recoilMultiplier);
    }

    public void FireServerWeaponVFX(Ray[] ray)
    {
        _playerWeaponController.FireServerWeaponVFX(ray);
    }
    
    #region  RPCs

    [ServerRpc]
    public void SetPlayerNameServerRPC(FixedString128Bytes username)
    {
        _userName.Value = username;
    }

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

    public void TakeDamage(ulong damageDealer, float damage)
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

    public void TakeSelfDamage(float damage)
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

    public void Heal(float health)
    {
        if (_health.Value > 0)
        {
            _health.Value += health;
            if (_health.Value > 100)
            {
                _health.Value = 100;
            }
        }
    }

    #endregion
}
