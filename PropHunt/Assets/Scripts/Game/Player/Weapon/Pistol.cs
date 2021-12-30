using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Pistol : Gun
{
    public override void Fire(Vector3 lookPoint, Vector3 lookDirection)
    {
        Ray ray = new Ray();
        ray.origin = lookPoint;
        ray.direction = lookDirection;

        FireServerRpc(ray);
        FireVFX(ray);

        base.Fire(lookPoint, lookDirection);
    }

    protected void FireVFX(Ray ray)
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
    }

    [ServerRpc]
    private void FireServerRpc(Ray ray)
    {
        if (Physics.Raycast(ray, out var raycastHit, _range, _hitLayers))
        {
            GameObject hitObject = raycastHit.collider.gameObject;

            PlayerController hitPlayerController = hitObject.GetComponentInParent<PlayerController>();

            if (hitPlayerController != null)
            {
                hitPlayerController.TakeDamageServerRpc(OwnerClientId, _gunDamage);
            }
            else
            {
                UserPlayerController.TakeSelfDamageServerRpc(_gunSelfDamage);
            }
        }
    }

    [ClientRpc]
    private void FireClientRpc(Ray ray)
    {
        if (IsOwner) return;

        SpawnMuzzleFlash();
        SpawnBulletEffect();
    }
}
