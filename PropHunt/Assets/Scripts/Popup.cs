using System;
using UnityEngine;

public abstract class Popup : MonoBehaviour
{
    public Action OnShow;
    public Action OnHide;
    public virtual void Show()
    {
        OnShow?.Invoke();
    }

    public virtual void Hide()
    {
        OnHide?.Invoke();
    }
}
