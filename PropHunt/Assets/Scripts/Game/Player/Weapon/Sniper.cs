using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Sniper : Gun
{
    public override void Fire(Vector3 lookPoint, Vector3 lookDirection)
    {
        Ray ray = new Ray();
        ray.origin = lookPoint;
        ray.direction = lookDirection;

        FireServerRpc(ray);

        if (IsOwner)
            FireVFX(new Ray[]{ray});

        base.Fire(lookPoint, lookDirection);
    }
}
