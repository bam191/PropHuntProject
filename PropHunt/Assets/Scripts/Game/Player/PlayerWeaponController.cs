using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerWeaponController : NetworkBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private PlayerAnimationController _animationController;
    [SerializeField] private PlayerGunController _localGunController;
    [SerializeField] private PlayerGunController _serverGunController;
    [SerializeField] private PlayerCameraController _cameraController;

    private Gun _equippedGun;
    private bool _isAiming;
    private bool _isTransitionAiming;

    public NetworkVariable<eGunType> _networkEquippedGun;

    private void Start()
    {
        if (!IsOwner)
        {
            EquipServerSideGun();
        }
    }

    private bool CanSwitchWeapons()
    {
        InputController.eInputState currentState = InputController.Instance.InputState;

        return currentState == InputController.eInputState.Hunter && !InputController.Instance.IsPaused;
    }

    private bool CanFireWeapon()
    {
        InputController.eInputState currentState = InputController.Instance.InputState;

        return currentState == InputController.eInputState.Hunter && !InputController.Instance.IsPaused;
    }

    private bool CanAimWeapon()
    {
        InputController.eInputState currentState = InputController.Instance.InputState;

        return currentState == InputController.eInputState.Hunter && !InputController.Instance.IsPaused;
    }

    private void Update()
    {
        if (!IsLocalPlayer) return;

        TrySwitchingWeapons();
        TryAimWeapon();
        TryFireWeapon();
    }

    private void TrySwitchingWeapons()
    {
        if (!CanSwitchWeapons()) return;

        if (InputManager.GetKeyDown(PlayerConstants.Weapon1))
        {
            EquipGun(eGunType.Pistol);
        }
        else if (InputManager.GetKeyDown(PlayerConstants.Weapon2))
        {
            EquipGun(eGunType.Rifle);
        }
        else if (InputManager.GetKeyDown(PlayerConstants.Weapon3))
        {
            EquipGun(eGunType.Shotgun);
        }
        else if (InputManager.GetKeyDown(PlayerConstants.Weapon4))
        {
            EquipGun(eGunType.Sniper);
        }
        else if (InputManager.GetKeyDown(PlayerConstants.Weapon5))
        {
            EquipGun(eGunType.RPG);
        }
    }

    private void TryAimWeapon()
    {
        if (!CanAimWeapon()) return;

        if (InputManager.GetKey(PlayerConstants.AimWeapon) && !_isAiming)
        {
            if (_equippedGun != null && _equippedGun.CanAim())
            {
                _isAiming = true;
                _equippedGun.EnterAiming();
            }
            //aim
        }
        else if (!InputManager.GetKey(PlayerConstants.AimWeapon) && _isAiming)
        {
            if (_equippedGun != null && _equippedGun.CanStopAim())
            {
                _isAiming = false;
                _equippedGun.ExitAiming();
            }
            //stop aim
        }
    }

    private void TryFireWeapon()
    {
        if (!CanFireWeapon()) return;

        if (_equippedGun != null && _equippedGun.GetFireMode() == eGunFireMode.FullAuto)
        {
            if (InputManager.GetKey(PlayerConstants.FireWeapon))
            {
                FireGun();
            }
        }
        else
        {
            if (InputManager.GetKeyDown(PlayerConstants.FireWeapon))
            {
                FireGun();
            }
        }
    }

    private void FireGun()
    {
        if (_equippedGun != null && _equippedGun.CanFire())
        {
            _equippedGun.Fire(_cameraController.FirstPersonCamera.gameObject.transform.position, _cameraController.FirstPersonCamera.gameObject.transform.forward, _cameraController.GetRecoilMultiplier());
            _playerController.AddRecoil(_equippedGun.GetRecoil(), _equippedGun.GetRecoilMultiplier());
        }
    }

    private void EquipGun(eGunType gunType)
    {
        if (_equippedGun == null || (_equippedGun.CanSwitch() && _equippedGun.GunType != gunType))
        {
            _equippedGun?.SwitchWeapon();
            _localGunController.SetGunState(gunType);
            _equippedGun = _localGunController.EquippedGun.GetComponent<Gun>();
            CrosshairController.Instance.SetCrosshairType(gunType);

            EquipGunServerRpc(gunType);
        }
    }

    [ServerRpc]
    private void EquipGunServerRpc(eGunType gunType)
    {
        _serverGunController.SetGunState(gunType);

        if (!IsLocalPlayer)
        {
            _equippedGun = _serverGunController.EquippedGun.GetComponent<Gun>();
        }

        _networkEquippedGun.Value = gunType;

        EquipGunClientRpc(gunType);
    }

    [ClientRpc]
    private void EquipGunClientRpc(eGunType gunType)
    {
        if(IsOwner) return;

        _serverGunController.SetGunState(gunType);

        if (!IsLocalPlayer)
        {
            _equippedGun = _serverGunController.EquippedGun.GetComponent<Gun>();
        }
    }

    private void EquipServerSideGun()
    {
        _serverGunController.SetGunState(_networkEquippedGun.Value);
        
        if (!IsLocalPlayer)
        {
            _equippedGun = _serverGunController.EquippedGun.GetComponent<Gun>();
        }
    }

    public void FireServerWeaponVFX(Ray[] ray)
    {
        _equippedGun.FireVFX(ray);
    }
}
