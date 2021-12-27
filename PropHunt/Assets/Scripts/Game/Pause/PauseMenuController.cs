using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
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

    [SerializeField] private Popup _settingsPopup;
    [SerializeField] private Popup _pausePopup;

    private void Awake()
    {
        SetState(ePauseMenuState.None);
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
}
