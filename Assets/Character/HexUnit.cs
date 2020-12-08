using UnityEngine;

public class HexUnit : MonoBehaviour
{
    public GameObject preview;
    HexCell location;
    public ParticleSystem selection;


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
            value.unit = this;
        }
    }




    public void setMaterial(Material material)
    {
        preview.GetComponent<MeshRenderer>().material = material;
    }

    public void Activate()
    {
        selection.Play();
    }

    public void Deactivate()
    {
        selection.Stop();
    }
}
