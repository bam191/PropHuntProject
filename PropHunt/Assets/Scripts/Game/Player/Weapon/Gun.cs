using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public enum eGunFireMode
{
    Single,
    SemiAuto,
    Burst,
    FullAuto
}

public class Gun : NetworkBehaviour
{
    [SerializeField] protected eGunType _gunType;
    [SerializeField] protected eGunFireMode _fireMode;
    [SerializeField] protected LayerMask _hitLayers;

    [SerializeField] protected float _gunDamage;
    [SerializeField] protected float _gunSelfDamage;
    [SerializeField] protected float _range;
    [SerializeField] protected float _fireRate;
    [SerializeField] protected float _recoilAmount;
    [SerializeField] protected float _recoilMultiplier;
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

    public virtual eGunFireMode GetFireMode()
    {
        return _fireMode;
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

    public virtual float GetRecoil()
    {
        return _recoilAmount;
    }

    public virtual float GetRecoilMultiplier()
    {
        return _recoilMultiplier;
    }

    protected virtual void FireVFX(Ray ray)
    {
        if (Physics.Raycast(ray, out var raycastHit, _range, _hitLayers))
        {
            GameObject hitObject = raycastHit.collider.gameObject;

            ObjectMaterial objectMaterial = hitObject.GetComponentInParent<ObjectMaterial>();
            ObjectMaterial.eMaterial hitMaterial = ObjectMaterial.eMaterial.None;
            if (objectMaterial == null)
            {
                objectMaterial = hitObject.GetComponentInChildren<ObjectMaterial>();
            }

            if (objectMaterial != null)
            {
                hitMaterial = objectMaterial.GetMaterial();
            }

            Vector3 hitPoint = raycastHit.point;
            Vector3 hitNormal = raycastHit.normal;

            VFXController.Instance.SpawnBulletHit(hitPoint, hitNormal, hitObject.transform, hitMaterial);
        }

        SpawnMuzzleFlash();
        SpawnBulletEffect();
    }

    [ServerRpc]
    protected virtual void FireServerRpc(Ray ray)
    {
        bool hitPlayer = false;
        float damageDealt = 0;

        if (Physics.Raycast(ray, out var raycastHit, _range, _hitLayers))
        {
            GameObject hitObject = raycastHit.collider.gameObject;

            PlayerController hitPlayerController = hitObject.GetComponentInParent<PlayerController>();

            if (hitPlayerController != null)
            {
                UserPlayerController.Heal(_gunSelfDamage);
                hitPlayerController.TakeDamage(OwnerClientId, _gunDamage);
                damageDealt = _gunDamage;
                hitPlayer = true;
            }
            else
            {
                UserPlayerController.TakeSelfDamage(_gunSelfDamage);
            }
        }

        FireClientRpc(ray, hitPlayer, damageDealt);
    }

    [ClientRpc]
    protected virtual void FireClientRpc(Ray ray, bool hitPlayer, float damageDealt)
    {
        if (IsOwner) return;

        FireVFX(ray);
    }
}
