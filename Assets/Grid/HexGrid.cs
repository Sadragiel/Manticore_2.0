using UnityEngine;
using UnityEngine.UI;
using Assets.DataStructures;
using System;
using System.Collections.Generic;

public class HexGrid : MonoBehaviour
{
	public int width = 14;
	public int height = 13;

	public HexCell cellPrefab;
	HexCell[] cells;

	public Text cellLabelPrefab;
	Canvas gridCanvas;

	HexMesh hexMesh;
	public Color defaultColor = Color.clear;


	//HexCell GetCellUnderCursor()
	//{
	//	Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
	//	RaycastHit hit;
	//	if (Physics.Raycast(inputRay, out hit))
	//	{
	//		return GetCell(hit.point);
	//	}
	//	return null;
	//}

	void Awake()
	{
		gridCanvas = GetComponentInChildren<Canvas>();
		hexMesh = GetComponentInChildren<HexMesh>();
		cells = new HexCell[height * width];
	}

	public void GenerateMap()
	{
		for (int z = 0, i = 0; z < height; z++)
		{
			for (int x = 0; x < width; x++)
			{
				if(z % 2 == 0 || x != width - 1)
                {
					CreateCell(x, z, i);
				}
				i++;
			}
		}
		SetupLocations();
		hexMesh.Triangulate(cells);
	}

	public HexCell GetCell(int i)
    {
		return cells[i];
    }

	public void ColorCell(Vector3 position, Color color)
	{
		HexCell cell = GetCell(position);
		cell.color = color;
		hexMesh.Triangulate(cells);
	}

    public void ColorCell(HexCell cell, Color color)
	{
		if (cell == null)
			return;
		cell.color = color;
		hexMesh.Triangulate(cells);
	}

	public HexCell GetCell(Vector3 position)
	{
		position = transform.InverseTransformPoint(position);
		HexCoordinates coordinates = HexCoordinates.FromPosition(position);
		int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
		return index >= 0 && index < cells.Length ? cells[index] : null;
	}

	public HexCell GetFreeCell(string name)
	{
		foreach(HexCell cell in cells)
        {
			if(
				cell != null
				&& cell.unit == null
				&& cell.name.Equals(name))
            {
				return cell;
            }
        }
		return null;
	}

	public List<HexCell> GetCellsByName(string name)
	{
		List<HexCell> list = new List<HexCell>();
		foreach (HexCell cell in cells)
		{
			if (cell != null && cell.name.Equals(name))
			{
				list.Add(cell);
			}
		}
		return list;
	}

	void CreateCell(int x, int z, int i)
	{
		Vector3 position;
		position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
		position.y = 0f;
		position.z = z * (HexMetrics.outerRadius * 1.5f);

		HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
		cell.transform.SetParent(transform, false);
		cell.transform.localPosition = position;
		cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
		cell.color = defaultColor;

		if (x > 0)
		{
			cell.SetNeighbor(HexDirection.W, cells[i - 1]);
		}
		if (z > 0)
		{
			if ((z & 1) == 0)
			{
				cell.SetNeighbor(HexDirection.SE, cells[i - width]);
				if (x > 0)
				{
					cell.SetNeighbor(HexDirection.SW, cells[i - width - 1]);
				}
			}
			else
			{
				cell.SetNeighbor(HexDirection.SW, cells[i - width]);
				if (x < width - 1)
				{
					cell.SetNeighbor(HexDirection.SE, cells[i - width + 1]);
				}
			}
		}


		Text label = Instantiate<Text>(cellLabelPrefab);
		label.rectTransform.SetParent(gridCanvas.transform, false);
		label.rectTransform.anchoredPosition =
			new Vector2(position.x, position.z);
		label.text = cell.coordinates.ToStringOnSeparateLines();
	}

	void SetupLocations()
    {
		foreach(Location location in DataController.Instance.locations)
        {
			foreach (int cellCoordinate in location.cells)
            {
				if(cells[cellCoordinate] != null)
                {
					cells[cellCoordinate].locationName = location.name;
					cells[cellCoordinate].name = location.name;
					cells[cellCoordinate].color = location.color;
					cells[cellCoordinate].defaultColor = location.color;
					cells[cellCoordinate].artifactStock = location.artifactStock;
					if(location.artifactHost == cellCoordinate)
                    {
						cells[cellCoordinate].UpdateArtifactsIcon();
                    }
					else
                    {
						cells[cellCoordinate].artifactHost = cells[location.artifactHost];
					}
				}
            }
        }

		// setting relationships within locations
		foreach (HexCell cell in cells)
        {
			cell?.SetAchievableWithinLocation();
        }
	}

	public HexCell getHexCellFromPointer()
	{
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(inputRay, out hit))
		{
			return GetCell(hit.point);
		}
		return null;
	}
}
