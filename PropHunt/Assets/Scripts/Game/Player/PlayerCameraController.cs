using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    private float sensitivityMultiplier;

    public Camera playerCamera;
    public Quaternion TargetRotation { private set; get; }

    private const float MAX_CAMERA_X_ROTATION = 90;
    private const float HALF_ROTATION = 180;
    private const float FULL_ROTATION = 360;
    private const float BASE_SENSITIVITY_MULTIPLIER = 10;

    private const float RECOIL_DECAY_ACCELERATION = 75;
    private const float RECOIL_MULTIPLIER_DECAY_ACCELERATION = 75;
    private const float MAX_RECOIL = 25;
    private const float MAX_SIDE_RECOIL = 15;
    private const float MAX_RECOIL_MULTIPLIER = 5;

    private Quaternion DefaultRotation = Quaternion.identity;

    private float _currentSideRecoil;
    private float _currentRecoil;
    private float _currentRecoilDecay;
    private float _recoilMultiplier;
    private float _recoilMultiplierDecay;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        TargetRotation = transform.rotation;
    }

    private void Start()
    {
        sensitivityMultiplier = OptionsPreferencesManager.GetSensitivity();
        playerCamera.fieldOfView = OptionsPreferencesManager.GetCameraFOV();
    }

    private bool CanMoveCamera()
    {
        InputController.eInputState inputState = InputController.Instance.InputState;

        return inputState == InputController.eInputState.Freecam
        || inputState == InputController.eInputState.Hunter
        || inputState == InputController.eInputState.Prop
        || inputState == InputController.eInputState.Spectate;
    }

    private void Update()
    {
        if (!CanMoveCamera()) return;

        if (Time.timeScale == 0)
        {
            return;
        }

        UpdateRotation();
        UpdateRecoil();

        CrosshairController.Instance.SetSeparationMultiplier(_recoilMultiplier);
    }

    private void UpdateRotation()
    {
        // Rotate the camera.
        var rotation = new Vector2(-Input.GetAxis(PlayerConstants.MouseY), Input.GetAxis(PlayerConstants.MouseX));
        var targetEuler = TargetRotation.eulerAngles + (Vector3)rotation * sensitivityMultiplier * BASE_SENSITIVITY_MULTIPLIER;
        if (targetEuler.x > HALF_ROTATION)
        {
            targetEuler.x -= FULL_ROTATION;
        }
        targetEuler.x = Mathf.Clamp(targetEuler.x, -MAX_CAMERA_X_ROTATION, MAX_CAMERA_X_ROTATION);
        TargetRotation = Quaternion.Euler(targetEuler);
    }

    private void UpdateRecoil()
    {
        _currentRecoilDecay += Time.deltaTime * RECOIL_DECAY_ACCELERATION;
        _recoilMultiplierDecay += Time.deltaTime * RECOIL_MULTIPLIER_DECAY_ACCELERATION;
        _currentRecoil -= Time.deltaTime * _currentRecoilDecay;

        if (_currentSideRecoil < 0)
        {
            _currentSideRecoil += Time.deltaTime * _currentRecoilDecay;
            if (_currentSideRecoil > 0)
            {
                _currentSideRecoil = 0;
            }
        }
        else if (_currentSideRecoil > 0)
        {
            _currentSideRecoil -= Time.deltaTime * _currentRecoilDecay;
            if (_currentSideRecoil < 0)
            {
                _currentSideRecoil = 0;
            }
        }

        _recoilMultiplier -= Time.deltaTime * _recoilMultiplierDecay;

        if (_recoilMultiplier < 0)
        {
            _recoilMultiplier = 0;
        }

        if (_currentRecoil < 0)
        {
            _currentRecoil = 0;
            _recoilMultiplier = 0;
        }
    }

    public float GetRecoilMultiplier()
    {
        return _recoilMultiplier;
    }

    public Quaternion GetRecoilRotation()
    {
        Quaternion rotation = TargetRotation * Quaternion.Euler(-_currentRecoil, _currentSideRecoil, 0);
        return rotation;
    }

    public void LateUpdate()
    {
        playerCamera.transform.rotation = GetRecoilRotation();

        Quaternion newRotation = TargetRotation;
        newRotation.eulerAngles = new Vector3(0, newRotation.eulerAngles.y, 0);
        transform.rotation = newRotation;
    }

    public void ResetTargetRotation(Quaternion target)
    {
        TargetRotation = target;
    }

    public void SetTargetRotation(Quaternion newRotation)
    {
        newRotation.eulerAngles = new Vector3(newRotation.eulerAngles.x, newRotation.eulerAngles.y, 0);
        TargetRotation = newRotation;
    }
    
    public void AddRecoil(float recoilAmount, float recoilMultiplier)
    {
        _recoilMultiplier += recoilMultiplier;
        _currentRecoil += recoilAmount * _recoilMultiplier;
        _currentSideRecoil += UnityEngine.Random.Range(-recoilAmount * recoilMultiplier, recoilAmount * recoilMultiplier);
        _currentRecoilDecay = _currentRecoil * 0.5f;
        _recoilMultiplierDecay = 0;

        _currentRecoil = Mathf.Clamp(_currentRecoil, 0, MAX_RECOIL);
        _currentSideRecoil = Mathf.Clamp(_currentSideRecoil, -MAX_SIDE_RECOIL, MAX_SIDE_RECOIL);
        _recoilMultiplier = Mathf.Clamp(_recoilMultiplier, 0, MAX_RECOIL_MULTIPLIER);
    }
}