using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    BuildingInfo buildingInfo;

    public BuildingInfo BuildingInfo
    {
        get
        {
            return buildingInfo;
        }
    }

    void Awake()
    {
        buildingInfo = new BuildingInfo(transform.GetChild(0).position, transform.GetChild(1).position);
    }
}
public struct BuildingInfo
{
    public Vector3 ll, rr;

    public BuildingInfo(Vector3 ll, Vector3 rr)
    {
        this.ll = ll;
        this.rr = rr;
    }
}
