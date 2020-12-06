using UnityEngine;

public class HexUnit : MonoBehaviour
{
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
}
