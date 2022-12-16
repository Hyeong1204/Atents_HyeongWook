using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// 길을 그리기 위한 타일 클래스(자동으로 적잘한 스프라이트로 변경해주는 클래스)
public class RoadTile : Tile
{
    enum AdjTilePostion : byte
    {
        None = 0,           // 0000 0000
        North = 1,          // 0000 0001
        East = 2,           // 0000 0010
        South= 4,           // 0000 0100
        West = 8            // 0000 1000
    }

    /// <summary>
    /// 타일이 배치될 때 주변 타일 상황에 자동으로 선택되어 보여질 스프라이트
    /// </summary>
    public Sprite[] sprites;

    /// <summary>
    /// 타일이 그려질 때 자동으로 호출이 되는 함수.(타일이 타일맵에 배치되면 타일에서 선택한 스프라이트 그리는데 그 때 자동으로 호출 됨)
    /// (지금 표시할 스프라이트에 맞게 다시 그리라고 신호를 보내는 역할)
    /// </summary>
    /// <param name="position">타일맵에서 타일이 그려지는 위치</param>
    /// <param name="tilemap">이 타일이 그렺질 타일 맵</param>
    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        for(int y = 0; y < 2; y++)
        {
            for (int x = 0; x < 2; x++)
            {
                Vector3Int location = new(position.x + x, position.y + y, position.z);      // 주변 8방향의 위치
                if (hasThisTile(tilemap, position))     // 주변 타일이 나와 같은지 확인
                {
                    tilemap.RefreshTile(location);      // 같은 종류면 갱신 시킨다.
                }
            }
        }
    }

    /// <summary>
    /// 타일에 대한 타일 랜더링 데이터(tileData)를 찾아서 전달
    /// (실제로 그려질 스프라이트를 결정)
    /// </summary>
    /// <param name="position"></param>
    /// <param name="tilemap"></param>
    /// <param name="tileData"></param>
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
    }

    /// <summary>
    /// 타일맵에서 지정된 위치가 같은 종류의 타일인지 확인
    /// </summary>
    /// <param name="tilemap">확인할 타일맵</param>
    /// <param name="position">확인할 위치</param>
    /// <returns>true면 같은 종류의 타일. false면 다른 종류의 타일</returns>
    bool hasThisTile(ITilemap tilemap, Vector3Int position)
    {
        // 타일맵에서 타일을 가져온 후 나와 같은지 확인
        return tilemap.GetTile(position) == this;
    }
}
