using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : Singleton<InputController>
{
    public enum eInputState
    {
        None,
        Freecam,
        Prop,
        PropFrozen,
        PropFreecam,
        Hunter,
        HunterFreecam,
        HunterFrozen,
        Spectate
    }

    private eInputState _currentInputState = eInputState.Hunter;
    private bool _isPaused;

    public eInputState InputState
    {
        get
        {
            return _currentInputState;
        }
    }

    public bool IsPaused
    {
        get
        {
            return _isPaused;
        }
    }

    public override void Initialize()
    {
        DontDestroyOnLoad(gameObject);
        UnlockCursor();
        base.Initialize();
    }

    public void SetInputState(eInputState inputState)
    {
        _currentInputState = inputState;
    }

    public void SetPaused(bool isPaused)
    {
        _isPaused = isPaused;

        if (isPaused)
        {
            UnlockCursor();
        }
        else
        {
            LockCursor();
        }
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

}

