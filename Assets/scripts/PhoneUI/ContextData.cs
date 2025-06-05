using UnityEngine;

[CreateAssetMenu(fileName = "Context", menuName = "Game Data/Context Data")]
public class ContextData : ScriptableObject
{
    [TextArea]
    public string Context;
}