using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour {
    public FriendData friendData;

    private void Awake() {
        if (friendData != null) {
            Transform imageTransform = transform.Find("ProfileMask/ProfileImage");
            Transform textTransform = transform.Find("ProfileName");
            if (imageTransform != null) {
                Image profileImage = imageTransform.GetComponent<Image>();
                if (profileImage != null) {
                    profileImage.sprite = friendData.profileImage;
                }
            }
            if (textTransform != null) {
                TextMeshProUGUI profileName = textTransform.GetComponent<TextMeshProUGUI>();
                if (profileName != null) {
                    profileName.text = friendData.friendName;
                }
                else {
                    Debug.Log("TMP불러오기 실패" + gameObject.name);
                }
            }
        }
    }
}
