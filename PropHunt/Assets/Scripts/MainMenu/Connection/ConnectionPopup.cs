using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ConnectionPopup : Popup
{
    private static ConnectionPopup _instance;
    public static ConnectionPopup Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<ConnectionPopup>(true);
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
