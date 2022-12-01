using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject cellprefab;

    public int width = 16;
    public int height = 16;

    Cell[] cells;

    private void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        cells = new Cell[width * height];
        int index = 0;
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                GameObject cellObj = Instantiate(cellprefab, this.transform);
                cellObj.transform.position = new Vector3(j * 0.65f, -i * 0.65f, 0);
                Cell cell = cellObj.GetComponent<Cell>();
                cell.ID = index;
                cells[index] = cell;
                index++;
            }
        }
    }
}
