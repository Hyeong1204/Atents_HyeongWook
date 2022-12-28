using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Test_TileMapAstar : MonoBehaviour
{
    public Tilemap background;
    public Tilemap obstacle;
    
    void Start()
    {
        //background.size.x;          // 타일 맵의 가로 크기
        //background.size.y;          // 타일맵의 세로 크기

        GridMap gridmap = new GridMap(background, obstacle);
    }
}
