using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager instance;

    [SerializeField] string csv_FileName;

    Dictionary<int, Dialgoue> dialogueDic = new Dictionary<int, Dialgoue>();
    public static bool isFinish = false;

    // ★ 총 대사 수 외부에서 참조 가능
    public int dialogueCount = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DialgoueParser theParser = GetComponent<DialgoueParser>();
            Dialgoue[] dialogues = theParser.Parse(csv_FileName);

            dialogueCount = dialogues.Length; // ★ 추가

            Debug.Log($"[DatabaseManager] Parsed dialogue count: {dialogueCount}");

            for (int i = 0; i < dialogues.Length; i++)
            {
                var d = dialogues[i];
                Debug.Log($"[{i + 1}] 이름: {d.name} / 문장 수: {d.contexts.Length}");
                for (int j = 0; j < d.contexts.Length; j++)
                {
                    Debug.Log($"   - 대사[{j}]: {d.contexts[j]} / 이벤트: {d.number[j]} / 스킵: {d.skipnum[j]}");
                }

                dialogueDic.Add(i + 1, d);
            }

            isFinish = true;
        }
    }

    public Dialgoue[] GetDialogue(int _StartNum, int _EndNum)
    {
        List<Dialgoue> dialogueList = new List<Dialgoue>();

        for (int i = 0; i <= _EndNum - _StartNum; i++)
        {
            dialogueList.Add(dialogueDic[_StartNum + i]);
        }

        return dialogueList.ToArray();
    }

    public void LoadDialogueFromCSV(string csvFileName)
    {
        dialogueDic.Clear();
        DialgoueParser theParser = GetComponent<DialgoueParser>();
        Dialgoue[] dialogues = theParser.Parse(csvFileName);

        dialogueCount = dialogues.Length; // ★ 추가

        for (int i = 0; i < dialogues.Length; i++)
        {
            dialogueDic.Add(i + 1, dialogues[i]);
        }

        isFinish = true;
    }
}
