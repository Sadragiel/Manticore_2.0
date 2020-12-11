using UnityEngine;
using Assets.Artifacts;
using System.Collections.Generic;

public class HexCell : MonoBehaviour
{
    public HexCoordinates coordinates;
    public Color color;
    public Color defaultColor;
    public ArtifactIcon artifactIcon;

    [SerializeField]
    HexCell[] neighbors;
    [SerializeField]
    public bool[] isNeighborAchievable;

    public HexUnit unit;
    public List<Artifact> artifactStock;
    public HexCell artifactHost;

    public string locationName;

    public Vector3 Position
    {
        get
        {
            return transform.localPosition;
        }
    }

    public HexCell GetNeighbor(HexDirection direction)
    {
        return neighbors[(int)direction];
    }

    public HexDirection? GetDirectionOfNeighbor(HexCell neighbor)
    {
        for(int i = 0; i < this.neighbors.Length; i++)
        {
            if (this.neighbors[i] == neighbor)
                return (HexDirection)i;
        }

        return null;
    }

    public void SetNeighbor(HexDirection direction, HexCell cell)
    {
        if (cell == null)
            return;
        neighbors[(int)direction] = cell;
        cell.neighbors[(int)direction.Opposite()] = this;
    }

    public void SetAchievableWithinLocation()
    {
        for (int i = 0; i < neighbors.Length; i++)
        {
            HexCell neighbor = neighbors[i];
            if (
                neighbor != null
                && IsLocation()
                && neighbor.locationName.Equals(locationName)
            )
            {
                isNeighborAchievable[i] = true;
            }
        }
    }

    public bool IsLocation()
    {
        return !locationName.Equals(DataController.Instance.emptyLocationName);
    }

    public bool IsActiveLocation()
    {
        if (!IsLocation())
            return false;
        var list = DataController.Instance.GetActiveLocationsList();
        return list.Contains(locationName);
    }

    public bool IsNeighborAchievable(HexDirection direction)
    {
        return isNeighborAchievable[(int)direction];
    }

    public bool IsNeighborAchievable(HexCell neighbor)
    {
        HexDirection? direction = this.GetDirectionOfNeighbor(neighbor);
        return direction != null && IsNeighborAchievable((HexDirection)direction);
    }

    public void SetAchievableNeighbors(bool[] directions)
    {
        isNeighborAchievable = directions;
        for(int i = 0; i < directions.Length; i++)
        {
            if (!directions[i])
                continue;
            HexDirection direction = (HexDirection)i;
            HexCell directedCell = GetNeighbor(direction);
            if (
                directedCell != null
                && directedCell.IsLocation())
            {
                directedCell.isNeighborAchievable[i] = true;
            }
        }
    }

    void Awake()
    {
        locationName = DataController.Instance.emptyLocationName;
        defaultColor = Color.white;
        artifactStock = new List<Artifact>();
    }

    public bool isBusy()
    {
        foreach (bool dir in isNeighborAchievable)
        {
            if (dir)
                return true;
        }
        return false;
    }

    public void UpdateArtifactsIcon()
    {
        HexCell host = artifactHost ?? this;
        if(artifactStock != null && artifactStock.Count != 0)
        {
            host.artifactIcon.setIconMaterial(artifactStock[0].material);
            host.artifactIcon.enabled = true;
        }
        else
        {
            host.artifactIcon.enabled = false;
        }
    }
}