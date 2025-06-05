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
