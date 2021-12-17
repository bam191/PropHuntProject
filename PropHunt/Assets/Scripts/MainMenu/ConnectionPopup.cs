using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionPopup : MonoBehaviour, IPopup
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

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
