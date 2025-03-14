using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.U2D;
using UnityEngine.UI;

public class NPCProcess : MonoBehaviour
{
    [SerializeField]
    public GameObject prefab;

    [SerializeField]
    public Transform parent;

    [SerializeField]
    public GameObject infoPanel;

    [SerializeField]
    public Image panelImage;

    [SerializeField]
    public TMP_Text panelName;

    [SerializeField]
    public TMP_Text panelText;

    [SerializeField]
    public Button panelCloseButton;

    private string[] Name = {
        "董施榮",
        "池宇楊",
        "張哲宇",
        "賴兆宏",
        "李柏勳",
        "張崇偉",
        "胡哲志",
        "張哲宇",
        "楊祐豪",
        "楊祐豪",
    };
    private string[] descriptions = {
        "這是第一個NPC的介紹",
        "這是第二個NPC的介紹",
        "這是第三個NPC的介紹",
        "這是第四個NPC的介紹",
        "這是第五個NPC的介紹",
        "這是第六個NPC的介紹",
        "這是第七個NPC的介紹",
        "這是第八個NPC的介紹",
        "這是第九個NPC的介紹",
        "這是第十個NPC的介紹",
    };
    private Image targetImage;
    private Button btn;
    void Start()
    {
        StartCoroutine(LoadImage());
        infoPanel.SetActive(false);
        panelCloseButton.onClick.AddListener(CloseInfoPanel);
    }
    
    IEnumerator LoadImage()
    {
        for (int i = 1; i <= 10 ; i++)
        {
            GameObject newPrefab = Instantiate(prefab, parent);
            btn = newPrefab.GetComponent<Button>();
            targetImage = newPrefab.transform.GetChild(0).GetComponent<Image>();

            string filePath = Path.Combine(Application.streamingAssetsPath, $"Image/NPC/{i}.jpg");
            using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(filePath, true))
            {
                yield return request.SendWebRequest();
                if (request.result == UnityWebRequest.Result.Success)
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(request);
                    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    targetImage.sprite = sprite;
                    int index = i;
                    btn.onClick.AddListener(() => OpenInfoPanel(index, sprite));
                }
            }
        }
    }


    void OpenInfoPanel(int index, Sprite sprite)
    {
        infoPanel.SetActive(true);
        panelImage.sprite = sprite;
        panelName.text = Name[index-1];
        panelText.text = descriptions[index-1];
    }

    public void CloseInfoPanel()
    {
        infoPanel.SetActive(false);
    }
}
