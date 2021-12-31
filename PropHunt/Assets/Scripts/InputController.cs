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
        HunterFrozen,
        Spectate
    }

    private eInputState _currentInputState = eInputState.Hunter;

    public eInputState InputState
    {
        get
        {
            return _currentInputState;
        }
    }

    public override void Initialize()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SetInputState(eInputState inputState)
    {
        _currentInputState = inputState;
    }
}

