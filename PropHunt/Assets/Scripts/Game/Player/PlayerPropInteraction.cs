using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerPropInteraction : MonoBehaviour
{
    private MeshFilter _myMeshFilter;
    private PlayerController _playerController;
    private PlayerCameraController _playerCameraController;
    
    private const float propInteractionDistance = 2;
    [SerializeField] private LayerMask propLayerMask;
    
    void Awake()
    {
        _playerController = GetComponentInParent<PlayerController>();
        _playerCameraController = GetComponent<PlayerCameraController>();
    }

    void Update()
    {
        // Only allow players on the "props" team to interact
        if (_playerController.team.Value != eTeam.Props)
        {
            //return;
        }
        
        if (InputManager.GetKeyDown(PlayerConstants.Interact))
        {
            Ray ray = _playerCameraController.playerCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var raycastHit, propInteractionDistance, propLayerMask))
            {
                GameObject propPrefab = Resources.Load<GameObject>($"Props/{raycastHit.collider.gameObject.name}");
                GameObject propInstance = Instantiate(propPrefab, transform);
                propInstance.transform.localPosition = Vector3.zero;
                
            }
        }
    }
}