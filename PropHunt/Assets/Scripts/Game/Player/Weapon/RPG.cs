using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RPG : Gun
{
    [SerializeField] private float _explosionRadius;

    public override void FireVFX(Ray[] rays)
    {
        foreach (Ray ray in rays)
        {
            if (Physics.Raycast(ray, out var raycastHit, _range, _hitLayers))
            {
                Vector3 hitPoint = raycastHit.point;
                VFXController.Instance.SpawnExplosion(hitPoint);
                SpawnBulletEffect(ray, hitPoint);
            }
            else
            {
                SpawnBulletEffect(ray, ray.origin + (ray.direction * _range));
            }
        }

        SpawnMuzzleFlash();
    }

    [ServerRpc]
    protected override void FireServerRpc(Ray ray)
    {
        bool hitPlayer = false;
        float damageDealt = 0;

        if (Physics.Raycast(ray, out var raycastHit, _range, _hitLayers))
        {
            Vector3 hitPoint = raycastHit.point;

            Collider[] hitColliders = Physics.OverlapSphere(hitPoint, _explosionRadius, _hitLayers);

            foreach (Collider hitCollider in hitColliders)
            {
                GameObject hitObject = raycastHit.collider.gameObject;

                PlayerController hitPlayerController = hitObject.GetComponentInParent<PlayerController>();

                if (hitPlayerController != null)
                {
                    UserPlayerController.Heal(_gunSelfDamage);
                    hitPlayerController.TakeDamage(OwnerClientId, _gunDamage);
                    damageDealt += _gunDamage;
                    hitPlayer = true;
                }
            }
        }

        if (!hitPlayer)
        {
            UserPlayerController.TakeSelfDamage(_gunSelfDamage);
        }

        FireClientRpc(new Ray[] { ray }, hitPlayer, damageDealt);
    }
}
