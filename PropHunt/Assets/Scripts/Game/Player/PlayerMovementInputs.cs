using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerMovementInputs
{
    public bool requestJump;
    public bool requestCrouch;
    public bool requestLeft;
    public bool requestRight;
    public bool requestBack;
    public bool requestForward;
    public float cameraYRotation;

    public Vector3 requestedPosition;
    public Vector3 requestedRotation;
}
