using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMaterial : MonoBehaviour
{
    public enum eMaterial
    {
        Concrete,
        Dirt,
        Metal,
        Sand,
        Wood,
        Flesh
    }

    [SerializeField] private eMaterial _material;

    public eMaterial GetMaterial()
    {
        return _material;
    }
}
