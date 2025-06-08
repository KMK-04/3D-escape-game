using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialgoueParser : MonoBehaviour
{
    public static string[] SplitCSV(string line)
    {
        List<string> result = new List<string>();
        bool inQuotes = false;
        string value = "";

        foreach (char c in line)
        {
            if (c == '\"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == ',' && !inQuotes)
            {
                result.Add(value);
                value = "";
            }
            else
            {
                value += c;
            }
        }

        result.Add(value); // 마지막 값 추가

        return result.ToArray();
    }

    public Dialgoue[] Parse(string _CSVFilieName)
    {
        List<Dialgoue> dialogueList = new List<Dialgoue>();
        TextAsset csvData = Resources.Load<TextAsset>(_CSVFilieName);

        if (csvData == null)
        {
            Debug.LogError($"CSV 파일을 찾을 수 없습니다: {_CSVFilieName}");
            return null;
        }

        string[] data = csvData.text.Split(new char[] { '\n' });

        Dialgoue currentDialogue = null;
        string currentName = "";
        List<string> contexts = new List<string>();
        List<string> numbers = new List<string>();
        List<string> skips = new List<string>();

        for (int i = 1; i < data.Length; i++)
        {
            string[] row = SplitCSV(data[i]);

            if (row.Length < 5 || string.IsNullOrWhiteSpace(row[1]))
                continue;

            string name = row[1];

            if (currentDialogue == null || currentName != name)
            {
                if (currentDialogue != null)
                {
                    currentDialogue.contexts = contexts.ToArray();
                    currentDialogue.number = numbers.ToArray();
                    currentDialogue.skipnum = skips.ToArray();
                    dialogueList.Add(currentDialogue);
                }

                currentDialogue = new Dialgoue();
                currentDialogue.name = name;
                currentName = name;

                // 새로운 리스트 초기화
                contexts = new List<string>();
                numbers = new List<string>();
                skips = new List<string>();
            }

            contexts.Add(row[2]);
            numbers.Add(row[3]);
            skips.Add(row[4]);
        }

        // 마지막 데이터 추가
        if (currentDialogue != null)
        {
            currentDialogue.contexts = contexts.ToArray();
            currentDialogue.number = numbers.ToArray();
            currentDialogue.skipnum = skips.ToArray();
            dialogueList.Add(currentDialogue);
        }

        return dialogueList.ToArray();
    }


}
