using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXController : Singleton<VFXController>
{
    private const int MAX_VFX = 50;

    [SerializeField] private VFXData _vfxData;

    public struct SpawnedVFXData
    {
        public float SpawnTime;
        public float LifeTime;
        public GameObject VFXObject;
    }

    private List<SpawnedVFXData> _spawnedVFX = new List<SpawnedVFXData>();

    public override void Initialize()
    {
        _spawnedVFX = new List<SpawnedVFXData>();
        DontDestroyOnLoad(gameObject);
        base.Initialize();
    }

    private void Update()
    {
        List<SpawnedVFXData> removeList = new List<SpawnedVFXData>();
        foreach(SpawnedVFXData vfxData in _spawnedVFX)
        {
            if (vfxData.SpawnTime + vfxData.LifeTime <= Time.time)
            {
                removeList.Add(vfxData);
                Destroy(vfxData.VFXObject);
            }
        }

        foreach(SpawnedVFXData vfxData in removeList)
        {
            _spawnedVFX.Remove(vfxData);
        }

        removeList.Clear();
    }

    public void SpawnBulletHit(Vector3 hitPoint, Vector3 normal, Transform parent, ObjectMaterial.eMaterial material)
    {
        GameObject bulletHit = Instantiate<GameObject>(_vfxData.GetBulletImpact(material));
        SpawnedVFXData vfxData = new SpawnedVFXData();
        vfxData.SpawnTime = Time.time;
        vfxData.VFXObject = bulletHit;
        vfxData.LifeTime = 20;

        bulletHit.transform.parent = parent;
        bulletHit.transform.position = hitPoint;
        bulletHit.transform.LookAt(hitPoint + normal);

        _spawnedVFX.Add(vfxData);

        CleanUpExtraVFX();
    }

    public void SpawnExplosion(Vector3 hitPoint)
    {
        GameObject explosion = Instantiate<GameObject>(_vfxData.GetExplosion(ObjectMaterial.eMaterial.None));
        SpawnedVFXData vfxData = new SpawnedVFXData();
        vfxData.SpawnTime = Time.time;
        vfxData.VFXObject = explosion;
        vfxData.LifeTime = 20;

        explosion.transform.position = hitPoint;

        _spawnedVFX.Add(vfxData);

        CleanUpExtraVFX();
    }

    public void SpawnMuzzleFlash(Transform gunBarrel, eGunType gunType)
    {
        GameObject muzzleFlash = Instantiate<GameObject>(_vfxData.GetMuzzleFlash(gunType));
        SpawnedVFXData vfxData = new SpawnedVFXData();
        vfxData.SpawnTime = Time.time;
        vfxData.VFXObject = muzzleFlash;
        vfxData.LifeTime = 1;

        muzzleFlash.transform.parent = gunBarrel;
        muzzleFlash.transform.position = gunBarrel.position;
        muzzleFlash.transform.rotation = gunBarrel.rotation;

        _spawnedVFX.Add(vfxData);

        CleanUpExtraVFX();
    }

    private void CleanUpExtraVFX()
    {
        if (_spawnedVFX.Count >= MAX_VFX)
        {
            int vfxCount = _spawnedVFX.Count - MAX_VFX;
            for (int i = 0; i < vfxCount; i++)
            {
                RemoveFirstVFX();
            }
        }
    }

    private void RemoveFirstVFX()
    {

    }
}
