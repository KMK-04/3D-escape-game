using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SaveTextureNameOnToggle : MonoBehaviour
{
    public Toggle toggleButton;       // Toggle 버튼 연결
    public RawImage targetRawImage;   // RawImage 오브젝트 연결

    private void Start()
    {
        toggleButton.onValueChanged.AddListener(OnToggleChanged);
    }

    private void OnToggleChanged(bool isOn)
    {
        if (isOn && targetRawImage != null && targetRawImage.texture != null)
        {
            string textureName = targetRawImage.texture.name; // Texture 파일 이름
            string path = Path.Combine(Application.persistentDataPath, "item.txt");
            File.WriteAllText(path, textureName);
            Debug.Log("Texture 이름이 저장되었습니다: " + textureName);
        }
    }
}
