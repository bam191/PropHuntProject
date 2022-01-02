using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NameplateController : MonoBehaviour
{
    [SerializeField] private TextMeshPro[] _namePlates;

    public void SetName(string name)
    {
        foreach(TextMeshPro textMeshPro in _namePlates)
        {
            textMeshPro.text = name;
        }
    }
}
