using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerPlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    
    void Start()
    {
        
    }

    void Update()
    {
        transform.position = _playerController.requestedPosition.Value;
        transform.rotation = Quaternion.Euler(_playerController.requestedRotation.Value);
    }
}
