using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerPlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;

    private void Update()
    {
        transform.position = _playerController._requestedPosition.Value;
        transform.rotation = Quaternion.Euler(new Vector3(0, _playerController._requestedRotation.Value.y, 0));
    }
}
