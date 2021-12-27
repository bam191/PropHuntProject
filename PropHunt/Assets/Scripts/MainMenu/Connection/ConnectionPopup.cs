using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ConnectionPopup : Popup
{
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
