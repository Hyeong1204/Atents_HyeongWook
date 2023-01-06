using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    /// <summary>
    /// 맵의 세로 개수
    /// </summary>
    const int heightCount = 3;

    /// <summary>
    /// 맵의 가로 개수
    /// </summary>
    const int widthCount = 3;

    const float mapHeightLength = 20.0f;
    const float mapWidthtLength = 20.0f;

    readonly Vector2 totalOrigin = new Vector2(-mapWidthtLength * widthCount * 0.5f, -mapHeightLength * heightCount * 0.5f);    // 맵 전체 길이의 절반

    const string SceneNameBase = "Seamless_";
    string[] sceneNames;

    /// <summary>
    /// 씬의 로딩 상태를 나타낸 enum
    /// </summary>
    enum SceneLoadState : byte
    {
        UnLoad = 0,         // 로딩이 안되어 있음
        PendingUnLoad,      // 로딩 해제중
        PendingLoad,        // 로딩 중
        Loaded              // 로딩 완료됨
    }

    /// <summary>
    /// 각 씬들의 로딩 상태
    /// </summary>
    SceneLoadState[] sceneLoadState;

    /// <summary>
    /// 초기화 함수 (단 한번만 실행)
    /// </summary>
    public void Initialize()
    {
        // 맵의 개수에 맞게 배열 생성
        sceneNames = new string[heightCount * widthCount];
        sceneLoadState = new SceneLoadState[heightCount * widthCount];

        for (int y = 0; y < heightCount; y++)
        {
            for (int x = 0; x < widthCount; x++)
            {
                int index = GetIndex(x, y);
                sceneNames[index] = $"{SceneNameBase}{y}_{x}";      // 각 신의 이름 설정
                sceneLoadState[index] = SceneLoadState.UnLoad;      // 각 씬의 로딩 상태 초기화
            }
        }

        Player player = GameManager.Inst.Player;
        player.onMapMoved += (position) => RefreshScenes(position.x, position.y);
        Vector2Int grid = WorldToGrid(player.transform.position);
        RequestAsyncSceneLoad(grid.x, grid.y);                      // 플레이어가 존재하는 맵이 최우선적으로 처리하기 위해 실행
        RefreshScenes(grid.x, grid.y);
    }

    /// <summary>
    /// x, y 그리드 좌표를 인덱스로 변환해주는 함수
    /// </summary>
    /// <param name="x">x 좌표</param>
    /// <param name="y">y 좌표</param>
    /// <returns>좌표에 해당하는 인덱스</returns>
    int GetIndex(int x, int y)
    {
        return x + widthCount * y;
    }

    /// <summary>
    /// 인덱스를 x, y 그리드 좌표로 변경해주는 함수
    /// </summary>
    /// <param name="index">인덱스 값</param>
    /// <returns>인덱스에 해당하는 그리드 좌표</returns>
    Vector2Int GetGrid(int index)
    {
        Vector2Int grid = new Vector2Int(index % widthCount, index / widthCount);
        return grid;
    }

    /// <summary>
    /// 지정된 좌표에 해당하는 맵을 비동기로 로딩 시작
    /// </summary>
    /// <param name="x">그리드 x</param>
    /// <param name="y">그리드 y</param>
    void RequestAsyncSceneLoad(int x, int y)
    {
        int index = GetIndex(x, y);                              // 인덱스 계산
        if (sceneLoadState[index] == SceneLoadState.UnLoad)      // 해당 맵이 Unloade상태일 때만 로딩 시도
        {
            AsyncOperation async = SceneManager.LoadSceneAsync(sceneNames[index], LoadSceneMode.Additive);  // 비동기 로딩 시작
            async.completed += (_) => sceneLoadState[index] = SceneLoadState.Loaded;                        // 로딩이 완료 되면 Loaded로 상태 변경
            sceneLoadState[index] = SceneLoadState.PendingLoad;  // 로딩 시작 표시
        }
    }

    /// <summary>
    /// 지정된 좌표에 해당하는 맵을 비동기로 로딩해제 시작
    /// </summary>
    /// <param name="x">그리드 x</param>
    /// <param name="y">그리드 y</param>
    void RequestAsyncSceneUnLoad(int x, int y)
    {
        int index = GetIndex(x, y);                                     // 인덱스 계산
        if (sceneLoadState[index] == SceneLoadState.Loaded)             // 해당 맵이 Unloade상태일 때만 로딩 시도
        {
            // 맵 언로드 전에 맵에 있는 슬라임들 처리
            Scene scene = SceneManager.GetSceneByName(sceneNames[index]);           // 언로드할 씬 가져오기
            GameObject[] sceneObjs = scene.GetRootGameObjects();                    // 씬에 있는 오브젝트를 가져오기
            if (sceneObjs.Length > 0)
            {
                Slime[] slimes = sceneObjs[0].GetComponentsInChildren<Slime>();     // 씬 구조를 알고 있기 때문에 첫 번째 오브젝트에서 모든 슬라임 가져오기
                foreach (var slime in slimes)
                {
                    slime.ClearData();                      // 데이터 정리
                    slime.gameObject.SetActive(false);      // 슬라임 비활성화 시키기
                }
            }

            AsyncOperation async = SceneManager.UnloadSceneAsync(sceneNames[index], UnloadSceneOptions.None);    // 비동기 로딩해제 시작
            async.completed += (_) => sceneLoadState[index] = SceneLoadState.UnLoad;                             // 로딩이 완료 되면 Unloade로 상태 변경
            sceneLoadState[index] = SceneLoadState.PendingUnLoad;       // 로딩해제 시작 표시
        }
    }

    /// <summary>
    /// 입력 받은 월드좌표가 어떤 그리드 좌표인지  알려주는 함수
    /// </summary>
    /// <param name="worldPos">확인할 월드좌표</param>
    /// <returns>변환된 그리드 좌표</returns>
    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        Vector2 offset = (Vector2)worldPos - totalOrigin;       // 전체맵의 원점에서 얼마나 떨어졌는지 계산
        return new Vector2Int((int)(offset.x / mapWidthtLength), (int)(offset.y / mapHeightLength));    // 몇 번째 맵에 해당되는지 확인
    }

    /// <summary>
    /// 지정된 목저지 주변은 로딩요청하고 그외는 전부 로딩해제 요청하는 함수
    /// </summary>
    /// <param name="x">지정된 grid x좌표</param>
    /// <param name="y">지정된 grid y좌표</param>
    void RefreshScenes(int x, int y)
    {
        int startX = Mathf.Max(0, x - 1);                           // 범위를 벗어나는 것을 방지하기 위해 미리 계산
        int endX = Mathf.Min(widthCount, x + 2);
        int startY = Mathf.Max(0, y - 1);
        int endY = Mathf.Min(heightCount, y + 2);

        List<Vector2Int> openList = new List<Vector2Int>(widthCount * heightCount);    // 로딩 된 지역 기록
        for (int _x = startX; _x < endX; _x++)
        {
            for (int _y = startY; _y < endY; _y++)
            {
                RequestAsyncSceneLoad(_x, _y);                      // 로딩한 곳들 로딩 됴청
                openList.Add(new(_x, _y));                         // 로딩한 지역 기록
            }
        }

        Vector2Int target = new Vector2Int();                       // 리스트에 찾는 값이 있는지 확인하기 위해 만든 임시 변수
        for (int _y = 0; _y < heightCount; _y++)                    // 모든 맵을 전부 처리
        {
            for (int _x = 0; _x < widthCount; _x++)
            {
                target.x = _x;
                target.y = _y;
                if (!openList.Exists((iter) => iter == target))    // openList에 없는 위치만
                {
                    RequestAsyncSceneUnLoad(_x, _y);                // 로딩 해제 요청
                }
            }
        }
    }

    void RefreshScenesMy(int x, int y)
    {
        List<Vector2Int> Removescenes = new List<Vector2Int>(8);
        for (int i = 0; i < heightCount; i++)
        {
            for (int j = 0; j < widthCount; j++)
            {
                Removescenes.Add(new Vector2Int(j, i));
            }
        }

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                Vector2Int grid = new Vector2Int(x + j, y + i);
                if (IsValidMapGrid(grid.x, grid.y))
                {
                    RequestAsyncSceneLoad(grid.x, grid.y);
                    Removescenes.Remove(grid);
                }
            }
        }

        foreach (var scene in Removescenes)
        {
            RequestAsyncSceneUnLoad(scene.x, scene.y);
        }
    }

    bool IsValidMapGrid(int x, int y)
    {
        return 0 <= x && x < widthCount && 0 <= y && y < heightCount;
    }

    // 테스트 용
    public void Test_LoadScene(int x, int y)
    {
        RequestAsyncSceneLoad(x, y);
    }

    public void Test_LoadUnScene(int x, int y)
    {
        RequestAsyncSceneUnLoad(x, y);
    }

    public void Test_RefreshScenes(int x, int y)
    {
        RefreshScenes(x, y);
    }

}
