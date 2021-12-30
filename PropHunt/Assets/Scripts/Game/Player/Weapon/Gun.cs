using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Gun : NetworkBehaviour
{
    [SerializeField] protected eGunType _gunType;
    [SerializeField] protected LayerMask _hitLayers;

    [SerializeField] protected float _gunDamage;
    [SerializeField] protected float _gunSelfDamage;
    [SerializeField] protected float _range;
    [SerializeField] protected float _fireRate;
    [SerializeField] protected float _recoilAmount;
    [SerializeField] protected float _switchTime;
    [SerializeField] protected float _aimTime;

    protected PlayerController _playerController;

    protected PlayerController UserPlayerController
    {
        get
        {
            if (_playerController == null)
            {
                _playerController = GetComponentInParent<PlayerController>();
            }

            return _playerController;
        }
    }

    protected float _lastFireTime;
    protected float _lastStartAimTime;
    protected float _lastEndAimTime;
    protected float _lastSwitchWeaponTime;


    public virtual void Fire(Vector3 lookPoint, Vector3 lookDirection)
    {
        _lastFireTime = Time.time;
    }

    protected virtual void SpawnMuzzleFlash()
    {

    }
    
    protected virtual void SpawnBulletEffect()
    {

    }

    public virtual void EnterAiming()
    {
        _lastStartAimTime = Time.time;
    }

    public virtual void ExitAiming()
    {
        _lastEndAimTime = Time.time;
    }

    public virtual void SwitchWeapon()
    {
        _lastSwitchWeaponTime = Time.time;
    }

    public virtual bool CanAim()
    {
        bool hasFinishedAiming = _lastStartAimTime + _aimTime <= Time.time;
        bool hasFinishedStopAiming = _lastEndAimTime + _aimTime <= Time.time;

        bool hasFinishedSwitching = _lastSwitchWeaponTime + _switchTime <= Time.time;

        return hasFinishedAiming && hasFinishedStopAiming && hasFinishedSwitching;
    }

    public virtual bool CanStopAim()
    {
        bool hasFinishedAiming = _lastStartAimTime + _aimTime <= Time.time;
        bool hasFinishedStopAiming = _lastEndAimTime + _aimTime <= Time.time;

        bool hasFinishedSwitching = _lastSwitchWeaponTime + _switchTime <= Time.time;

        return hasFinishedAiming && hasFinishedStopAiming && hasFinishedSwitching;
    }

    public virtual bool CanSwitch()
    {
        return _lastSwitchWeaponTime + _switchTime <= Time.time;
    }

    public virtual float GetDamageDealt()
    {
        return _gunDamage;
    }

    public virtual float GetDamageTaken()
    {
        return _gunSelfDamage;
    }

    public virtual bool CanFire()
    {
        float timeBetweenShots = 1f / _fireRate;

        bool hasFinishedFiring = _lastFireTime + timeBetweenShots <= Time.time;
        bool hasFinishedSwitching = _lastSwitchWeaponTime + _switchTime <= Time.time;

        return hasFinishedFiring && hasFinishedSwitching;
    }
}
