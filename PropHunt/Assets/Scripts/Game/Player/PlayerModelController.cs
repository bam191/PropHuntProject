using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelController : MonoBehaviour
{
    private const string baseHunterModel = "Props/BaseHunterModel";
    private const string basePropModel = "Props/BasePropModel";
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetTeam(eTeam team)
    {
        if (team == eTeam.Hunters)
        {
            SetModel(baseHunterModel);
        }
        else
        {
            SetModel(basePropModel);
        }
    }

    public void SetModel(string resourceLocation)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        
        GameObject propPrefab = Resources.Load<GameObject>(resourceLocation);
        GameObject propInstance = Instantiate(propPrefab, transform);
        propInstance.transform.localPosition = Vector3.zero;
    }
}
