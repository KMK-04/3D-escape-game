using UnityEngine;

[CreateAssetMenu(fileName = "NewFriendData", menuName = "Game Data/Friend Data")]
public class FriendData : ScriptableObject
{
    public string friendName;
    public Sprite profileImage;
    [TextArea(15, 80)]
    public string Prompts;
}