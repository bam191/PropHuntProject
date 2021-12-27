using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private GameObject _camera;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private CameraMove _cameraMove;
    [SerializeField] private CharacterController _characterController;
    private void Start()
    {
        if (!IsLocalPlayer)
        {
            _camera.SetActive(false);
            _cameraMove.enabled = false;
            _characterController.enabled = false;
        }
    }
}
