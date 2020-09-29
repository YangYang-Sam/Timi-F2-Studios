using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour
{
	public static HexGrid instance;
	public int width = 6;
	public int height = 6;
	public int Radius = 4;

	public HexCell cellPrefab;

    public HexCell[] cells;
	public Text cellLabelPrefab;

	Canvas gridCanvas;

	HexMesh hexMesh;

    public Color[] DefaultColors;


	public Vector3Int other=new Vector3Int(2,-5,3);
	void Awake()
	{
		instance = this;
		gridCanvas = GetComponentInChildren<Canvas>();
		hexMesh = GetComponentInChildren<HexMesh>();

		gridCanvas = GetComponentInChildren<Canvas>();

		cells = new HexCell[height * width];


	}
    private void Start()
    {
		Random.InitState(UserData.instance.RandomSeeds[0]);
		int cellIndex = 0;
		for (int z = 0, i = 0; z < height; z++)
		{
			for (int x = 0; x < height; x++)
			{
				int y = -(x - z / 2) - z;
				int Dx = Mathf.Abs(x - z / 2 - other.x);
				int Dy = Mathf.Abs(y - other.y);
				int Dz = Mathf.Abs(z - other.z);
				int distance = (Dx + Dy + Dz) / 2;
				HexCell cell = CreateCell(x, z, i++);
				cell.HexIndex = cellIndex;
				cellIndex++;
				if (distance >= Radius)
				{
					ColorCell(cell, new Color(0.4f, 0.4f, 0.4f));
					cell.CanPass = false;
				}
			}
		}
		hexMesh.Triangulate(cells);
        foreach (HexCell cell in cells)
        {
			cell.GetNearbyCells();
        }
	}
	HexCell CreateCell(int x, int z, int i)
	{
		Vector3 position;
		position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
		position.y = 0f;
		position.z = z * (HexMetrics.outerRadius * 1.5f);

		HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
		cell.transform.SetParent(transform, false);
		cell.transform.position = position;
		cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
				
        int typeIndex = Random.Range(0, 6);
        cell.CellType = (HexCellType)typeIndex;
		cell.color = DefaultColors[typeIndex];

		string t = "";
        switch ((HexCellType)typeIndex)
        {
			case HexCellType.Desert:
				t = "沙";
				break;
			case HexCellType.Forest:
				t = "森";
				break;
			case HexCellType.Grass:
				t = "草";
				break;
			case HexCellType.Hill:
				t = "丘";
				break;
			case HexCellType.Water:
				t = "湖";
				break;
			case HexCellType.Snow:
				t = "雪";
				break;
			case HexCellType.Montain:
				t = "山";
				break;
		}
		cell.TypeText.text = t;

		GameObject terrain = Instantiate(AssetsManager.instance.HexTerrainPrefab[typeIndex]);
		terrain.transform.position = cell.transform.position;
      


        //Text label = Instantiate<Text>(cellLabelPrefab);
        //label.rectTransform.SetParent(gridCanvas.transform, false);
        //label.rectTransform.anchoredPosition =
        //	new Vector2(position.x, position.z);
        //label.text = cell.coordinates.ToStringOnSeparateLines();

        return cell;
	}


	public void ColorCell(HexCell cell, Color color)
	{		
		cell.color = color;
		hexMesh.Triangulate(cells);
	}

	public HexCell GetCellByPosition(Vector3 position)
    {
		//position = transform.InverseTransformPoint(position);
		HexCoordinates coordinates = HexCoordinates.FromPosition(position);
		int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
		return cells[index];
	}
}
