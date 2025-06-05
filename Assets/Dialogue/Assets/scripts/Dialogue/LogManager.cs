using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _logs;

    private Text[] _logTxts = new Text[100];
    public int Log_index;

    // Start is called before the first frame update
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Add_Log(string[] message)
    {
        for(int i = 0; i < message.Length; i++)
        {
            _logTxts[Log_index].text += message[i];
        }
        
        _logs[Log_index].SetActive(true);
        Log_index++;
    }

    public void Init_Log()
    {
        for (int i = 0; i < _logs.Length; i++)
        {
            _logTxts[i] = _logs[i].GetComponent<Text>();
            _logTxts[i].text = "";
            _logs[i].SetActive(false);
        }
        Log_index = 0;
    }
}
