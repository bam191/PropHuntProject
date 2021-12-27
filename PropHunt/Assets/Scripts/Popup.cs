using System;
using UnityEngine;

public abstract class Popup : MonoBehaviour
{
    public Action OnShow;
    public Action OnHide;
    public bool isShown;
    
    public virtual void Show()
    {
        if (isShown)
        {
            return;
        }
        
        isShown = true;
        OnShow?.Invoke();
    }

    public virtual void Hide()
    {
        if (!isShown)
        {
            return;
        }
        
        isShown = false;
        OnHide?.Invoke();
    }
}
