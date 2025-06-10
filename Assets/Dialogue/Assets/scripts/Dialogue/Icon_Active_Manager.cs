using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icon_Active_Manager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] ON_panels;
    [SerializeField]
    private GameObject[] OFF_panels;

    public void On_Panel()
    {
        for (int i = 0; i < ON_panels.Length; i++)
        {
            if (ON_panels[i].name == "Dialogue_Panel")
            {
                // 일반 대화 매니저 체크
                if (Dialogue_Manage.Instance != null && Dialogue_Manage.Instance.isEndLine())
                {
                    return;
                }
                
                // 엔딩 대화 매니저 체크
                if (EndingDialogueManager.Instance != null && EndingDialogueManager.Instance.isEndLine())
                {
                    return;
                }
            }
            ON_panels[i].SetActive(true);
        }
    }
    
    public void Off_Panel()
    {
        for(int i = 0; i < OFF_panels.Length; i++)
        {      
            OFF_panels[i].SetActive(false);          
        }
    }
}