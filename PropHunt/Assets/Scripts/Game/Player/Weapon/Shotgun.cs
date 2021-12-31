using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Shotgun : Gun
{
    [SerializeField] private int _pellets;
    
    public override void Fire(Vector3 lookPoint, Vector3 lookDirection)
    {
        Ray ray = new Ray();
        ray.origin = lookPoint;
        ray.direction = lookDirection;

        List<Ray> pelletRays = new List<Ray>();

        for (int i = 0; i < _pellets; i++)
        {
            Ray pelletRay = new Ray(ray.origin, ray.direction + (Random.insideUnitSphere * 0.1f));
            pelletRays.Add(pelletRay);
        }

        FireServerRpc(pelletRays.ToArray());

        if (IsOwner)
            FireVFX(pelletRays.ToArray());

        base.Fire(lookPoint, lookDirection);
    }

    [ServerRpc]
    protected void FireServerRpc(Ray[] pelletRays)
    {
        bool hitPlayer = false;
        int playerHits = 0;
        float damageDealt = 0;

        foreach(Ray pelletRay in pelletRays)
        {
            if (Physics.Raycast(pelletRay, out var raycastHit, _range, _hitLayers))
            {
                GameObject hitObject = raycastHit.collider.gameObject;

                PlayerController hitPlayerController = hitObject.GetComponentInParent<PlayerController>();

                if (hitPlayerController != null)
                {
                    hitPlayerController.TakeDamage(OwnerClientId, _gunDamage);
                    hitPlayer = true;
                    damageDealt += _gunDamage;
                    playerHits++;
                }
            }
        }

        int misses = _pellets - playerHits;

        for (int i = 0; i < playerHits;i++)
        {
            UserPlayerController.Heal(_gunSelfDamage);
        }

        for (int i = 0; i < misses;i++)
        {
            UserPlayerController.TakeSelfDamage(_gunSelfDamage);
        }

        FireClientRpc(pelletRays, hitPlayer, damageDealt);
    }
}
