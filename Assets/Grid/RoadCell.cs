using System;
using UnityEngine;
using Assets.DataStructures;

public class RoadCell : MonoBehaviour
{
    public GameObject self;

    HexCell location;
    public HexCell Location
    {
        get
        {
            return location;
        }
        set
        {
            location = value;
            transform.localPosition = value.Position;
        }
    }

    public void setMaterial (Material material)
    {
        self.GetComponent<MeshRenderer>().material = material;
    }

    public void Rotate(float direction)
    {
        this.transform.Rotate(0, 60 * Math.Sign(direction), 0);
    }
}
