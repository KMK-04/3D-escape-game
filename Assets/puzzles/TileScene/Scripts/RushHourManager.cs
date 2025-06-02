using UnityEngine;

public class RushHourManager : MonoBehaviour
{
    public ChestController chest;   // 상자 컨트롤러 참조

    // 퍼즐 완료 후 이 함수 호출
    public void OnPuzzleSolved()
    {
        chest.GiveNote();           // 쪽지 지급(ChestController에 만들어 둔다고 가정)
        CloseMiniGame();
    }

    // X 버튼에서 호출
    public void CloseMiniGame()
    {
        gameObject.SetActive(false);
    }
}
