using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RPG : Gun
{
    [SerializeField] private float _explosionRadius;

    public override void Fire(Vector3 lookPoint, Vector3 lookDirection)
    {
        Ray ray = new Ray();
        ray.origin = lookPoint;
        ray.direction = lookDirection;

        FireServerRpc(ray);

        if (IsOwner)
            FireVFX(ray);

        base.Fire(lookPoint, lookDirection);
    }

    protected override void FireVFX(Ray ray)
    {
        if (Physics.Raycast(ray, out var raycastHit, _range, _hitLayers))
        {
            Vector3 hitPoint = raycastHit.point;
            VFXController.Instance.SpawnExplosion(hitPoint);
        }

        SpawnMuzzleFlash();
        SpawnBulletEffect();
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

            foreach(Collider hitCollider in hitColliders)
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

        FireClientRpc(ray, hitPlayer, damageDealt);
    }

    [ClientRpc]
    protected override void FireClientRpc(Ray ray, bool hitPlayer, float damageDealt)
    {
        base.FireClientRpc(ray, hitPlayer, damageDealt);
    }
}
