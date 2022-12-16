using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// 길을 그리기 위한 타일 클래스(자동으로 적잘한 스프라이트로 변경해주는 클래스)
public class RoadTile : Tile
{
    [Flags]         // 이 enum은 비트 플래그로 사용하겠다.
    enum AdjTilePostion : byte      // 8비트 크기의 enum
    {
        None = 0,           // 0000 0000    주변에 RoadTile이 없다
        North = 1,          // 0000 0001    북쪽에 RoadTile이 있다
        East = 2,           // 0000 0010    동쪽에 RoadTile이 있다
        South = 4,           // 0000 0100    남쪽에 RoadTile이 있다
        West = 8,           // 0000 1000    서쪽에 RoadTile이 있다
        All = North | East | South | West    // 0000 1111 모든 방향에 RoadTile이 있다.
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
    /// <param name="tilemap">이 타일이 그려질 타일 맵</param>
    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        for (int y = -1; y < 2; y++)
        {
            for (int x = -1; x < 2; x++)
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
    /// <param name="position">타일맵에서 타일 데이터를 가져올 타일의 타일 맵에서의 위치</param>
    /// <param name="tilemap">타일 데이터를 가져올 타일맵</param>
    /// <param name="tileData">가져온 타일 데이터의 참조(읽기, 쓰기 가능)</param>
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        AdjTilePostion mask = AdjTilePostion.None;
        // mask에 주변 타일의 상황을 기록하기
        // ex) 북쪽에 RoadTile이 있으면 mask에는 AdjTilePostion.North가 들어가야 한다.
        //     북동쪽에 RoadTile이 있으면 mask에는 (AdjTilePostion.North|AdjTilePostion.East)가 들어가야 한다.

        //if (hasThisTile(tilemap, new Vector3Int(0,1,0)))
        //{
        //    mask = mask | AdjTilePostion.North;
        //}

        mask |= hasThisTile(tilemap, position + new Vector3Int(0, 1, 0)) ? AdjTilePostion.North : 0;
        mask |= hasThisTile(tilemap, position + new Vector3Int(1, 0, 0)) ? AdjTilePostion.East : 0;
        mask |= hasThisTile(tilemap, position + new Vector3Int(0, -1, 0)) ? AdjTilePostion.South : 0;
        mask |= hasThisTile(tilemap, position + new Vector3Int(-1, 0, 0)) ? AdjTilePostion.West : 0;

        // mask값에 따라 어떤 스프라이트를 보여 줄 것인지 결정
        // mask갑에 따라 얼마만큼 회전 시킬 것인지 결정
    }

    int GetIndex(AdjTilePostion mask)
    {
        return -1;
    }

    Quaternion GetRotation(AdjTilePostion mask)
    {
        return Quaternion.identity;
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
