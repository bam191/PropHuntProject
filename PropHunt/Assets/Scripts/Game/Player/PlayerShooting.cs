using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private CameraMove _cameraMove;
    [SerializeField] private const float shootDistance = 1000;
    [SerializeField] private LayerMask playerLayerMask;
    
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(("shooting"));
            Ray ray = _cameraMove.playerCamera.ScreenPointToRay(Input.mousePosition);

            Fire(ray);
        }
    }
    
    #region  RPCs

    [ServerRpc]
    public void Fire(Ray ray)
    {
        if (Physics.Raycast(ray, out var raycastHit, shootDistance, playerLayerMask))
        {
            NetworkObject networkObject = raycastHit.collider.gameObject.GetComponent<NetworkObject>();

            if (networkObject != null)
            {
                InformPlayerHit();
                Debug.Log($"enemy hit!: {networkObject.OwnerClientId}");
            }
            else
            {
                Debug.LogError("Hit an object on the player layer without a network object script attached");
            }
        }
    }

    [ClientRpc]
    private void InformPlayerHit()
    {
        
    }

    #endregion
}
