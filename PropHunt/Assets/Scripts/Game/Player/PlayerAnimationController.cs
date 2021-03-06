using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class PlayerAnimationController : NetworkBehaviour
{
    private const string VERTICAL_AIM_KEY = "VerticalAim";
    private const string FORWARD_SPEED_KEY = "ForwardSpeed";
    private const string RIGHT_SPEED_KEY = "RightSpeed";
    private const string FIRST_PERSON_KEY = "IsFirstPerson";
    private const string RIFLE_KEY = "HoldingRifle";
    private const string PISTOL_KEY = "HoldingPistol";

    [SerializeField] private Animator _localHunterAnimator;
    [SerializeField] private Animator _serverHunterAnimator;
    [SerializeField] private NetworkAnimator _serverHunterNetworkAnimator;
    [SerializeField] private PlayerController _playerController;


    private NetworkVariable<float> _networkVerticalAim;
    private NetworkVariable<float> _networkForwardSpeed;
    private NetworkVariable<float> _networkRightSpeed;
    private NetworkVariable<bool> _networkIsHoldingRifle;

    private Vector3 _lastPosition;

    private float _verticalAim = 0.5f;
    private Vector3 _velocity = Vector3.zero;
    private float _forwardSpeed = 0f;
    private float _rightSpeed = 0f;
    private bool _isHoldingRifle = true;

    public void UpdateLastPosition(Vector3 lastPosition)
    {
        _lastPosition = lastPosition;
    }

    private void Start()
    {
        _serverHunterAnimator.SetBool(FIRST_PERSON_KEY, false);

        _localHunterAnimator.SetBool(FIRST_PERSON_KEY, true);
        _localHunterAnimator.SetLayerWeight(_localHunterAnimator.GetLayerIndex("VerticalAim"), 0);
    }

    private void Update()
    {
        if (IsLocalPlayer && IsOwner)
        {
            float playerLookRotation = _playerController._requestedRotation.Value.x;
            if (playerLookRotation > 90) playerLookRotation -= 360;

            _verticalAim = Mathf.Lerp(0f, 1f, (playerLookRotation + 90f) / 180f);

            float forwardSpeed = Vector3.Dot(_velocity, Quaternion.Euler(0, _playerController._requestedRotation.Value.y, 0) * Vector3.forward);
            float rightSpeed = Vector3.Dot(_velocity, Quaternion.Euler(0, _playerController._requestedRotation.Value.y, 0) * Vector3.right);

            _forwardSpeed = Mathf.Lerp(-1f, 1f, (forwardSpeed + PlayerConstants.MoveSpeed) / (PlayerConstants.MoveSpeed * 2));
            _rightSpeed = Mathf.Lerp(-1f, 1, (rightSpeed + PlayerConstants.MoveSpeed) / (PlayerConstants.MoveSpeed * 2));

            SetAnimationValuesServerRpc(_forwardSpeed, _rightSpeed, _isHoldingRifle, _verticalAim);
        }
    }

    [ServerRpc]
    private void SetAnimationValuesServerRpc(float forwardSpeed, float rightSpeed, bool isHoldingRifle, float verticalAim)
    {
        _networkForwardSpeed.Value = forwardSpeed;
        _networkRightSpeed.Value = rightSpeed;
        _networkIsHoldingRifle.Value = isHoldingRifle;
        _networkVerticalAim.Value = verticalAim;
    }

    private void LateUpdate()
    {
        if (IsLocalPlayer)
        {
            _localHunterAnimator.SetBool(RIFLE_KEY, _isHoldingRifle);
            _localHunterAnimator.SetBool(PISTOL_KEY, !_isHoldingRifle);
            _localHunterAnimator.SetFloat(VERTICAL_AIM_KEY, 0.5f);
            _localHunterAnimator.SetLayerWeight(_localHunterAnimator.GetLayerIndex("VerticalAim"), 0);
        }
        else
        {
            _serverHunterAnimator.SetBool(RIFLE_KEY, _networkIsHoldingRifle.Value);
            _serverHunterAnimator.SetBool(PISTOL_KEY, !_networkIsHoldingRifle.Value);

            _serverHunterAnimator.SetFloat(VERTICAL_AIM_KEY, _networkVerticalAim.Value);
            _serverHunterAnimator.SetFloat(FORWARD_SPEED_KEY, _networkForwardSpeed.Value);
            _serverHunterAnimator.SetFloat(RIGHT_SPEED_KEY, _networkRightSpeed.Value);
        }
    }

    public void SetVelocity(Vector3 velocity)
    {
        _velocity = velocity;
    }

    public void SetGunType(eGunType gunType)
    {
        switch(gunType)
        {
            case eGunType.Rifle:
            case eGunType.Shotgun:
            case eGunType.RPG:
            case eGunType.Sniper:
                _isHoldingRifle = true;
                break;
            case eGunType.Pistol:
            case eGunType.None:
                _isHoldingRifle = true;
                break;
        }
    }
}
