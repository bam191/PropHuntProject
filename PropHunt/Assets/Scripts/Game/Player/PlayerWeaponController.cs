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

    private bool _canSwitchWeapons = true;

    private Gun _equippedGun;

    private bool CanSwitchWeapons()
    {
        InputController.eInputState currentState = InputController.Instance.InputState;

        return currentState == InputController.eInputState.Hunter;
    }

    private void Update()
    {
        if (!IsLocalPlayer || !CanSwitchWeapons()) return;

        if (InputController.Instance.InputState == InputController.eInputState.Hunter)

        if(InputManager.GetKeyDown(PlayerConstants.Weapon1))
        {
            Debug.LogError("equip pistol");
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

    private void EquipGun(eGunType gunType)
    {
        if (!_canSwitchWeapons) return;

        _localGunController.SetGunState(gunType);
        _equippedGun = _localGunController.EquippedGun.GetComponent<Gun>();

        EquipGunServerRpc(gunType);
    }

    [ServerRpc]
    private void EquipGunServerRpc(eGunType gunType)
    {
        _serverGunController.SetGunState(gunType);
        _equippedGun = _serverGunController.EquippedGun.GetComponent<Gun>();
    }
}
