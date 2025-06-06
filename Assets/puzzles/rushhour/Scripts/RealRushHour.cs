using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.SceneManagement;
public class RealRushHour : MonoBehaviour
{
    public int boardSize = 6;
    private Block[,] grid;
    public string targetSceneName; // 이 변수는 사용하지 않고 GameManager에서 원래 씬 이름을 가져옴
    public int exitRow = 2;
    public int exitCol;
    public static bool RushHourClear = false;
    public GameObject clearUI;
    public GameObject activatorObject;
    public Camera rushHourCamera;
    public Camera mainCamera;
    public GameObject rushHourRoom;
    public GameObject rushHourUI;
    public GameObject exitButton;
    public Transform playerTransform; // 플레이어 Transform (Inspector에서 할당)

    void Start()
    {
        Debug.Log("3");
        // 그리드 초기화
        grid = new Block[boardSize, boardSize];

        // 출구 열 위치 설정 (보드 오른쪽 끝)
        exitCol = boardSize - 1;

        // 초기 배치된 블록 자동으로 읽어서 Grid 배열에 저장
        foreach (var block in GetComponentsInChildren<Block>())
        {
            PlaceBlock(block, block.row, block.col);
        }

        PrintGrid(); // 디버깅용 그리드 상태 출력
    }

    // 블록이 이동 가능한지 확인
    public bool CanMove(Block block, int delta)
    {
        int newRow = block.row;
        int newCol = block.col;

        // 이동 방향에 따라 새 위치 계산
        if (block.isHorizontal)
            newCol += delta;
        else
            newRow += delta;

        // 이동 가능 여부 확인
        for (int i = 0; i < block.length; i++)
        {
            int r = newRow + (block.isHorizontal ? 0 : i);
            int c = newCol + (block.isHorizontal ? i : 0);

            // 보드 밖으로 나가는 경우
            if (r < 0 || r >= boardSize || c < 0 || c >= boardSize)
            {
                // 예외: 목표 블록이 출구로 나가는 경우는 허용
                if (block.isGoal && r == exitRow && c == exitCol)
                    continue;

                Debug.Log($"이동 불가: 보드 밖 - {block.name}, r={r}, c={c}");
                return false;
            }

            // 다른 블록과 충돌하는 경우
            if (grid[r, c] != null && grid[r, c] != block)
            {
                Debug.Log($"이동 불가: 충돌 - {block.name}과 {grid[r, c].name}");
                return false;
            }
        }

        return true;
    }

    // 블록 이동
    public bool MoveBlock(Block block, int delta)
{
    if (!CanMove(block, delta))
        return false;

    // ① 이전 위치에서 블록 제거
    RemoveBlock(block);

    // ② 블록 row/col 값 갱신
    if (block.isHorizontal)
        block.col += delta;
    else
        block.row += delta;

    // ③ 갱신된 위치로 다시 등록
    PlaceBlock(block, block.row, block.col);

    // ④ 화면상의 Transform 갱신
    block.UpdatePosition();

    // ⑤ 목표 블록 클리어 체크 (생략)
    if (block.isGoal)
        CheckGameClear(block);

    return true;
}

    // 목표 블록이 출구에 도달했는지 확인하는 메서드
    void CheckGameClear(Block goalBlock)
    {
        // 목표 블록의 오른쪽 끝이 출구 위치에 도달했는지 확인
        if (goalBlock.isHorizontal && goalBlock.col + goalBlock.length - 1 == exitCol && goalBlock.row == exitRow)
        {
            Debug.Log("퍼즐 클리어!");
            StartCoroutine(HandleGameClear(goalBlock));
        }
    }

    // 게임 클리어 처리
    IEnumerator HandleGameClear(Block goalBlock)
    {
        // 목표 블록 애니메이션 (출구로 빠져나가는 모습)
        float animTime = 1.0f;
        Vector3 startPos = goalBlock.transform.localPosition;
        Vector3 endPos = startPos + new Vector3(1.0f, 0, 0); // 오른쪽으로 한 칸 더 이동

        float elapsed = 0;
        while (elapsed < animTime)
        {
            goalBlock.transform.localPosition = Vector3.Lerp(startPos, endPos, elapsed / animTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 1. 퍼즐 클리어 플래그 방출
        RushHourClear = true;
        Debug.Log("RushHour 퍼즐 클리어 플래그 방출!");
        Debug.Log("2");
        yield return new WaitForSeconds(1.0f);

        // 2. 카메라/방/UI 복구 및 Activator 삭제
        RestoreMainScene(true);
        RestoreMainScene();


        // 3. GameManager에 결과 보고
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.GetBooleanListSize() > 0)
                GameManager.Instance.SetBoolean(0, true);
            else
                GameManager.Instance.AddBoolean(true);

            Debug.Log("GameManager booleanList[0]을 true로 설정 완료");

            // 👉 씬 복귀 요청
            GameManager.Instance.ReturnToOriginalScene();
        }
        else
        {
            Debug.LogWarning("GameManager 인스턴스가 존재하지 않습니다. 씬 복귀 불가");
        }
    }
    // 블록을 그리드에 배치
    void PlaceBlock(Block block, int row, int col)
    {
        for (int i = 0; i < block.length; i++)
        {
            int r = row + (block.isHorizontal ? 0 : i);
            int c = col + (block.isHorizontal ? i : 0);

            if (r >= 0 && r < boardSize && c >= 0 && c < boardSize)
            {
                grid[r, c] = block;
            }
        }
    }

    // 블록을 그리드에서 제거
    void RemoveBlock(Block block)
    {
        for (int r = 0; r < boardSize; r++)
        {
            for (int c = 0; c < boardSize; c++)
            {
                if (grid[r, c] == block)
                {
                    grid[r, c] = null;
                }
            }
        }
    }

    // 디버깅용 그리드 상태 출력
    public void PrintGrid()
    {
        string gridStr = "현재 그리드 상태:\n";
        for (int r = 0; r < boardSize; r++)
        {
            for (int c = 0; c < boardSize; c++)
            {
                if (r == exitRow && c == exitCol)
                    gridStr += "X ";
                else if (grid[r, c] == null)
                    gridStr += "□ ";
                else
                    gridStr += (grid[r, c].isGoal ? "★ " : "■ ");
            }
            gridStr += "\n";
        }
        Debug.Log(gridStr);
    }

    // 카메라/방/UI 복구 함수 및 Activator 삭제
    public void RestoreMainScene(bool removeActivator = false)
    {
        if (rushHourRoom != null) rushHourRoom.SetActive(false);
        if (rushHourCamera != null) rushHourCamera.gameObject.SetActive(false);
        if (rushHourUI != null) rushHourUI.SetActive(false);
        if (mainCamera != null) mainCamera.gameObject.SetActive(true);

        // 클리어 시에만 Activator 삭제
        if (removeActivator && activatorObject != null)
            Destroy(activatorObject);
    }

    public void OnEnable()
    {
        if (exitButton != null)
            exitButton.SetActive(true);
    }

    public void OnDisable()
    {
        if (exitButton != null)
            exitButton.SetActive(false);
    }
}
