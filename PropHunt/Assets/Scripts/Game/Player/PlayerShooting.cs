using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerShooting : NetworkBehaviour
{
    [SerializeField] private const float shootDistance = 1000;
    [SerializeField] private LayerMask playerLayerMask;

    private PlayerCameraController _playerCameraController;
    private Gun _playerGun;

    void Start()
    {
        _playerCameraController = GetComponent<PlayerCameraController>();
    }
    
    public void Fire()
    {
        Ray ray = _playerCameraController.playerCamera.ViewportPointToRay(new Vector3(0.5f,0.5f,0));

        if (Physics.Raycast(ray, out var raycastHit, shootDistance, playerLayerMask))
        {
            NetworkObject networkObject = raycastHit.collider.gameObject.GetComponentInParent<NetworkObject>();

            if (networkObject != null)
            {
                UpdateHealthServerRPC(1, networkObject.OwnerClientId);
            }
            else
            {
                Debug.LogError("Hit an object on the player layer without a network object script attached");
            }
        }
    }
    
    #region  RPCs
    [ServerRpc]
    private void UpdateHealthServerRPC(int damage, ulong clientId)
    {
        var clientToDamage = NetworkManager.Singleton.ConnectedClients[clientId]
            .PlayerObject.GetComponent<PlayerController>();

        if (clientToDamage != null && clientToDamage._health.Value > 0)
        {
            clientToDamage._health.Value -= damage;
        }
        
        // Execute method on the client getting hit
        NotifyHealthChangedClientRPC(damage, new ClientRpcParams()
        {
            Send = new ClientRpcSendParams()
            {
                TargetClientIds = new ulong[] {clientId}
            }
        });
        
    }

    [ClientRpc]
    public void NotifyHealthChangedClientRPC(int damage, ClientRpcParams clientRpcParams = default)
    { 
        // Don't notify myself
        if (IsOwner)
        {
            return;
        }
        
        NetworkLog.LogInfoServer($"Client got hit \ndamage:{damage}");
    }

    #endregion
}
