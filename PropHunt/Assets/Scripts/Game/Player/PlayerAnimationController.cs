using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private const string VERTICAL_AIM_KEY = "VerticalAim";
    private const string WALK_SPEED_KEY = "WalkSpeed";
    private const string FIRST_PERSON_KEY = "FirstPerson";
    private const string RIFLE_KEY = "HoldingRifle";
    private const string PISTOL_KEY = "HoldingPistol";

    [SerializeField] private Animator _localHunterAnimator;
    [SerializeField] private Animator _serverHunterAnimator;
    [SerializeField] private NetworkAnimator _serverHunterNetworkAnimator;
    [SerializeField] private PlayerController _playerController;

    private Vector3 _lastPosition;

    private float _veritcalAim = 0.5f;
    private Vector3 _velocity = Vector3.zero;
    private float _walkAnimationMultiplier = 1f;
    private bool _isHoldingRifle = false;

    public void UpdateLastPosition(Vector3 lastPosition)
    {
        _lastPosition = lastPosition;
    }

    private void Start()
    {
        _localHunterAnimator.SetBool(FIRST_PERSON_KEY, true);
        _serverHunterAnimator.SetBool(FIRST_PERSON_KEY, false);
    }

    private void Update()
    {
        Debug.LogError(_playerController._requestedRotation.Value.x);
        float playerLookRotation = _playerController._requestedRotation.Value.x;
        if (playerLookRotation > 90) playerLookRotation -= 360;
        
        _veritcalAim = Mathf.Lerp(0f, 1f, (_playerController._requestedRotation.Value.x + 90f) / 180f);
        _walkAnimationMultiplier = Vector3.Dot(_velocity, new Vector3(0, _playerController._requestedRotation.Value.y, 0));
    }

    private void LateUpdate()
    {
        if (_playerController.IsLocalPlayer)
        {
            _localHunterAnimator.SetBool(RIFLE_KEY, _isHoldingRifle);
            _localHunterAnimator.SetBool(PISTOL_KEY, !_isHoldingRifle);
        }

        if (_serverHunterNetworkAnimator.IsOwner)
        {
            _serverHunterAnimator.SetFloat(VERTICAL_AIM_KEY, _veritcalAim);
            _serverHunterAnimator.SetFloat(WALK_SPEED_KEY, _walkAnimationMultiplier);

            _serverHunterAnimator.SetBool(RIFLE_KEY, _isHoldingRifle);
            _serverHunterAnimator.SetBool(PISTOL_KEY, !_isHoldingRifle);
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
                _isHoldingRifle = false;
                break;
        }
    }
}
