using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;

public class SaveTextureNameOnToggle : MonoBehaviour
{
    public Toggle toggleButton;
    public RawImage targetRawImage;

    private void Start()
    {
        toggleButton.onValueChanged.AddListener(OnToggleChanged);
    }

    private void OnToggleChanged(bool isOn)
    {
        if (isOn && targetRawImage != null && targetRawImage.texture != null)
        {
            string textureName = targetRawImage.texture.name;
            // 프로젝트 폴더 기준 경로 지정
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Assets/item.txt");
            File.WriteAllText(path, textureName, Encoding.UTF8);
            Debug.Log("Texture 이름이 저장되었습니다: " + textureName);
            Debug.Log("저장 경로: " + path);
        }
    }
}
