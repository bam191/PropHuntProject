using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VFXData", menuName = "ScriptableObjects/VFXData", order = 1)]
public class VFXData : ScriptableObject
{
    public enum eVFXElementType
    {
        None,
        BulletImpact,
        MuzzleFlare,
        PointEffect
    }

    [Serializable]
    public struct BulletImpact
    {
        public ObjectMaterial.eMaterial MaterialType;
        public GameObject Prefab;
    }

    [Serializable]
    public struct Explosion
    {
        public ObjectMaterial.eMaterial MaterialType;
        public GameObject Prefab;
    }

    [Serializable]
    public struct MuzzleFlash
    {
        public eGunType GunType;
        public GameObject Prefab;
    }

    [Serializable]
    public struct BulletTrail
    {
        public eGunType GunType;
        public GameObject Prefab;
    }

    [SerializeField] private List<BulletImpact> _bulletImpacts = new List<BulletImpact>();
    [SerializeField] private List<Explosion> _explosions = new List<Explosion>();
    [SerializeField] private List<MuzzleFlash> _muzzleFlashes = new List<MuzzleFlash>();
    [SerializeField] private List<BulletTrail> _bulletTrails = new List<BulletTrail>();

    public GameObject GetBulletImpact(ObjectMaterial.eMaterial materialType)
    {
        foreach(BulletImpact dataElement in _bulletImpacts)
        {
            if (dataElement.MaterialType == materialType || materialType == ObjectMaterial.eMaterial.None)
            {
                return dataElement.Prefab;
            }
        }

        Debug.LogError($"Couldn't find bullet impact for type material {materialType}");
        return null;
    }

    public GameObject GetExplosion(ObjectMaterial.eMaterial materialType)
    {
        foreach(Explosion dataElement in _explosions)
        {
            if (dataElement.MaterialType == materialType || materialType == ObjectMaterial.eMaterial.None)
            {
                return dataElement.Prefab;
            }
        }

        Debug.LogError($"Couldn't find explosion for type material {materialType}");
        return null;
    }

    public GameObject GetMuzzleFlash(eGunType gunType)
    {
        foreach (MuzzleFlash dataElement in _muzzleFlashes)
        {
            if (dataElement.GunType == gunType || gunType == eGunType.None)
            {
                return dataElement.Prefab;
            }
        }

        Debug.LogError($"Couldn't find muzzle flash for type material {gunType}");
        return null;
    }

    public GameObject GetBulletTrail(eGunType gunType)
    {
        foreach (BulletTrail dataElement in _bulletTrails)
        {
            if (dataElement.GunType == gunType || gunType == eGunType.None)
            {
                return dataElement.Prefab;
            }
        }

        Debug.LogError($"Couldn't find muzzle flash for type material {gunType}");
        return null;
    }
}
