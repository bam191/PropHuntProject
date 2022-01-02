using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private GameObject _activeCamera;

    private void Start()
    {
        LookAtController.Instance.RegisterLookAt(this);
    }

    private void LateUpdate()
    {
        UpdateRotation();
    }

    public void SetActiveCamera(GameObject camera)
    {
        _activeCamera = camera;
    }

    private void UpdateRotation()
    {
        if (_activeCamera == null)
        {
            return;
        }

        transform.LookAt(_activeCamera.transform);
    }
}
