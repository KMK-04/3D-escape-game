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
            // ������Ʈ ���� ���� ��� ����
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Assets/item.txt");
            File.WriteAllText(path, textureName, Encoding.UTF8);
            Debug.Log("Texture �̸��� ����Ǿ����ϴ�: " + textureName);
            Debug.Log("���� ���: " + path);
        }
    }
}
