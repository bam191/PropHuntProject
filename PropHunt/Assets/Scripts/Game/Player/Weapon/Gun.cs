using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Gun : NetworkBehaviour
{
    [SerializeField] protected eGunType _gunType;
    
    [SerializeField] protected float _gunDamage;
    [SerializeField] protected float _gunSelfDamage;
    [SerializeField] protected float _range;
    [SerializeField] protected float _fireRate;
    [SerializeField] protected float _recoilAmount;

    
    public virtual void Shoot()
    {

    }

    protected virtual void SpawnMuzzleFlash()
    {

    }

    public virtual void EnterAiming()
    {

    }

    public virtual void ExitAiming()
    {

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
        return false;
    }
}
