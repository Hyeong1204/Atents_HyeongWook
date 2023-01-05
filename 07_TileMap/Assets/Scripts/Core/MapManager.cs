using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    /// <summary>
    /// 맵의 세로 개수
    /// </summary>
    const int Height = 3;

    /// <summary>
    /// 맵의 가로 개수
    /// </summary>
    const int width = 3;
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
        sceneNames = new string[Height * width];
        sceneLoadState = new SceneLoadState[Height * width];

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = GetIndex(x, y);
                sceneNames[index] = $"{SceneNameBase}{y}_{x}";      // 각 신의 이름 설정
                sceneLoadState[index] = SceneLoadState.UnLoad;      // 각 씬의 로딩 상태 초기화
            }
        }
    }

    /// <summary>
    /// x, y 그리드 좌표를 인덱스로 변환해주는 함수
    /// </summary>
    /// <param name="x">x 좌표</param>
    /// <param name="y">y 좌표</param>
    /// <returns>좌표에 해당하는 인덱스</returns>
    int GetIndex(int x, int y)
    {
        return x + width * y;
    }

    /// <summary>
    /// 인덱스를 x, y 그리드 좌표로 변경해주는 함수
    /// </summary>
    /// <param name="index">인덱스 값</param>
    /// <returns>인덱스에 해당하는 그리드 좌표</returns>
    Vector2Int GetGrid(int index)
    {
        Vector2Int grid = new Vector2Int(index % width, index / width);
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
            AsyncOperation async = SceneManager.UnloadSceneAsync(sceneNames[index], UnloadSceneOptions.None);    // 비동기 로딩해제 시작
            async.completed += (_) => sceneLoadState[index] = SceneLoadState.UnLoad;                             // 로딩이 완료 되면 Unloade로 상태 변경
            sceneLoadState[index] = SceneLoadState.PendingUnLoad;       // 로딩해제 시작 표시
        }
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
}
