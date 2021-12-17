using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePopup : Popup
{
    private static PausePopup _instance;
    public static PausePopup Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<PausePopup>(true);
            }

            return _instance;
        }
    }

    public override void Show()
    {
        gameObject.SetActive(true);
        base.Show();
    }

    public override void Hide()
    {
        gameObject.SetActive(false);
        base.Hide();
    }
}
