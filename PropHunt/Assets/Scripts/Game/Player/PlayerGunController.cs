using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public enum eGunType
{
    None,
    Pistol,
    Rifle,
    Shotgun,
    RPG,
    Sniper
}

public class PlayerGunController : NetworkBehaviour
{

    [SerializeField] private GameObject _pistol;
    [SerializeField] private GameObject _rifle;
    [SerializeField] private GameObject _shotgun;
    [SerializeField] private GameObject _rpg;
    [SerializeField] private GameObject _sniper;

    public NetworkVariable<eGunType> _currentGun;
    private eGunType _equippedGun = eGunType.None;

    private void Awake()
    {
        SetGunState(eGunType.None);
        _currentGun.OnValueChanged += GunStateChanged;
    }

    private void GunStateChanged(eGunType previousValue, eGunType newValue)
    {
        SetGunState(newValue);
    }

    public void SetGunState(eGunType gunType)
    {
        _pistol.SetActive(false);
        _rifle.SetActive(false);
        _shotgun.SetActive(false);
        _rpg.SetActive(false);
        _sniper.SetActive(false);

        switch (gunType)
        {
            case eGunType.Pistol:
                _pistol.SetActive(true);
                break;
            case eGunType.Rifle:
                _rifle.SetActive(true);
                break;
            case eGunType.Shotgun:
                _shotgun.SetActive(true);
                break;
            case eGunType.RPG:
                _rpg.SetActive(true);
                break;
            case eGunType.Sniper:
                _sniper.SetActive(true);
                break;
        }
    }
}
