using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SaveTextureNameOnToggle : MonoBehaviour
{
    public Toggle toggleButton;       // Toggle ��ư ����
    public RawImage targetRawImage;   // RawImage ������Ʈ ����

    private void Start()
    {
        toggleButton.onValueChanged.AddListener(OnToggleChanged);
    }

    private void OnToggleChanged(bool isOn)
    {
        if (isOn && targetRawImage != null && targetRawImage.texture != null)
        {
            string textureName = targetRawImage.texture.name; // Texture ���� �̸�
            string path = Path.Combine(Application.persistentDataPath, "item.txt");
            File.WriteAllText(path, textureName);
            Debug.Log("Texture �̸��� ����Ǿ����ϴ�: " + textureName);
        }
    }
}
