using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : Singleton<PauseMenuController>
{
    public enum ePauseMenuState
    {
        PauseMenu,
        SettingsPopup,
        None
    }

    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _quitButton;

    private ePauseMenuState _currentState = ePauseMenuState.PauseMenu;

    public ePauseMenuState CurrentState
    {
        get
        {
            return _currentState;
        }
    }

    [SerializeField] private Popup _pausePopup;

    private Popup _settingsPopup;
    [SerializeField] private Transform _settingsParent;
    [SerializeField] private GameObject _settingsPopupPrefab;

    public override void Initialize()
    {
        InitSettings();
        SetState(ePauseMenuState.None);

        base.Initialize();
    }

    private void OnDestroy()
    {
        InputController.Instance.SetPaused(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(PlayerConstants.PauseMenu))
        {
            switch(_currentState)
            {
                case ePauseMenuState.None:
                    SetState(ePauseMenuState.PauseMenu);
                    break;
                case ePauseMenuState.SettingsPopup:
                    SetState(ePauseMenuState.PauseMenu);
                    break;
                case ePauseMenuState.PauseMenu:
                    SetState(ePauseMenuState.None);
                    break;
            }
        }
    }

    public void SetState(ePauseMenuState state)
    {
        if (_currentState == state) return;

        _pausePopup.Hide();
        _settingsPopup.Hide();

        switch (state)
        {
            case ePauseMenuState.PauseMenu:
                _pausePopup.Show();
                break;
            case ePauseMenuState.SettingsPopup:
                _settingsPopup.Show();
                break;
        }

        if (_currentState == ePauseMenuState.None && state != ePauseMenuState.None)
        {
            InputController.Instance.SetPaused(true);
        }
        else if (state == ePauseMenuState.None)
        {
            InputController.Instance.SetPaused(false);
        }

        _currentState = state;
    }

    public void SetState(int state)
    {
        SetState((ePauseMenuState)state);
    }

    public void OnSettingsButtonPressed()
    {
        SetState(ePauseMenuState.SettingsPopup);
    }
    
    public void OnQuitButtonPressed()
    {

    }

    private void InitSettings()
    {
        GameObject settingsInstance = Instantiate(_settingsPopupPrefab, _settingsParent);
        _settingsPopup = settingsInstance.GetComponent<SettingsPopup>();
        _settingsPopup.OnHide += () => SetState(ePauseMenuState.PauseMenu);
    }
}
