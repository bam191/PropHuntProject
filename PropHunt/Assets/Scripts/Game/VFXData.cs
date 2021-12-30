using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VFXData", menuName = "ScriptableObjects/VFXData", order = 1)]
public class VFXData : ScriptableObject
{
    public enum eVFXElementType
    {
        None,
        BulletImpact,
        MuzzleFlare,
        PointEffect
    }

    [Serializable]
    public struct VFXDataElement
    {
        public eVFXElementType ElementType;
        public ObjectMaterial.eMaterial MaterialType;
        public GameObject Prefab;
    }

    [SerializeField] private List<VFXDataElement> _vfxDataElements = new List<VFXDataElement>();


    public GameObject GetGameObject(eVFXElementType elementType, ObjectMaterial.eMaterial materialType)
    {
        foreach(VFXDataElement dataElement in _vfxDataElements)
        {
            if (dataElement.ElementType == elementType)
            {
                if (dataElement.MaterialType == materialType || dataElement.MaterialType == ObjectMaterial.eMaterial.None || materialType == ObjectMaterial.eMaterial.None)
                {
                    return dataElement.Prefab;
                }
            }
        }

        Debug.LogError($"Couldn't find vfx element for type {elementType} and material {materialType}");
        return null;
    }

}
