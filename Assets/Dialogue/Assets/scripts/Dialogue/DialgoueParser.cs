using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialgoueParser : MonoBehaviour
{
    public Dialgoue[] Parse(string _CSVFilieName)
    {
        List<Dialgoue> dialgoueList = new List<Dialgoue>();
        TextAsset csvData = Resources.Load<TextAsset>(_CSVFilieName);

        if (csvData == null)
        {
            Debug.LogError($"CSV 파일을 찾을 수 없습니다: {_CSVFilieName}");
            return null;
        }

        string[] data = csvData.text.Split(new char[] { '\n' });

        for (int i = 1; i < data.Length;)
        {
            string[] row = data[i].Split(new char[] { ',' });

            // 빈 줄 또는 길이 부족한 줄 방지
            if (row.Length < 5 || string.IsNullOrWhiteSpace(row[1]) || string.IsNullOrWhiteSpace(row[2]))
            {
                i++;
                continue;
            }

            Dialgoue dialgoue = new Dialgoue();
            dialgoue.name = row[1];

            List<string> contextList = new List<string>();
            List<string> eventList = new List<string>();
            List<string> skipList = new List<string>();

            do
            {
                if (row.Length < 5)
                    break;

                contextList.Add(row[2]);
                eventList.Add(row[3]);
                skipList.Add(row[4]);

                i++;
                if (i < data.Length)
                {
                    row = data[i].Split(new char[] { ',' });
                }
                else
                {
                    break;
                }

            } while (string.IsNullOrWhiteSpace(row[0]));

            dialgoue.contexts = contextList.ToArray();
            dialgoue.number = eventList.ToArray();
            dialgoue.skipnum = skipList.ToArray();

            dialgoueList.Add(dialgoue);
        }

        return dialgoueList.ToArray();
    }
}
