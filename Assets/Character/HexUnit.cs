using UnityEngine;
using DG.Tweening;
using Assets.DataStructures;
using Assets.Artifacts;
using System.Collections.Generic;

public class HexUnit : MonoBehaviour
{
    public GameObject preview;
    HexCell location;
    public ParticleSystem selection;

    protected HexGrid grid
    {
        get
        {
            return GameManager.Instance.hexGrid;
        }
    }

    protected bool isMyTurn;
    public List<Artifact> bag;
    public List<Artifact> equipped;

    // STATS
    public int MAX_HP;
    public int HP;
    public int MAX_STAMINA;
    public int STAMINA;
    public int ATK;
    public int MAGIC_ATK;
    public int RANGE_ATK;
    public int DEF;
    public int MAGIC_DEF;


    public HexCell Location
    {
        get
        {
            return location;
        }
        set
        {
            if(location != null)
            {
                location.unit = null;
                transform.DOMove(value.transform.position, .9f);
            }
            else
            {
                transform.localPosition = value.Position;
            }
            location = value;
            value.unit = this;
        }
    }

    protected void Awake()
    {
        bag = new List<Artifact>();
        equipped = new List<Artifact>();
    }

    public void SetMetadata(CharacterMeta metadata)
    {
        MAX_HP = metadata.MAX_HP;
        HP = metadata.MAX_HP;
        MAX_STAMINA = metadata.MAX_STAMINA;
        STAMINA = metadata.MAX_STAMINA;
        ATK = metadata.ATK;
        MAGIC_ATK = metadata.MAGIC_ATK;
        RANGE_ATK = metadata.RANGE_ATK;
        DEF = metadata.DEF;
        MAGIC_DEF = metadata.MAGIC_DEF;
        preview.GetComponent<MeshRenderer>().material = metadata.material;
    }

    protected void RestoreStamina()
    {
        STAMINA = MAX_STAMINA;
    }

    public void TakeTurn()
    {
        RestoreStamina();
        isMyTurn = true;
        selection.Play();
    }

    public void EndTurn()
    {
        isMyTurn = false;
        selection.Stop();
        GameManager.Instance.EndTurn();
    }
}
