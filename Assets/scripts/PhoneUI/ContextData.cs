using UnityEngine;

[CreateAssetMenu(fileName = "Context", menuName = "Game Data/Context Data")]
public class ContextData : ScriptableObject {
    [TextArea(15, 80)]
    public string Context;
}