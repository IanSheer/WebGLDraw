using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class LoadHomework : MonoBehaviour
{
    private Image homework;

    private void Start()
    {
        homework = GetComponent<Image>();
        //GetImage.GetImageFromUserAsync(gameObject.name, "ReceiveImage");
    }

    public void DrawJSLayout()
    {
        //GetImage.GetImageFromUserAsync(gameObject.name, "ReceiveImage");
    }

    static string s_dataUrlPrefix = "data:image/png;base64,";
    public void ReceiveImage(string dataUrl)
    {
        if (dataUrl.StartsWith(s_dataUrlPrefix))
        {
            byte[] pngData = System.Convert.FromBase64String(dataUrl.Substring(s_dataUrlPrefix.Length));

            Texture2D tex = new Texture2D(1, 1);
            if (tex.LoadImage(pngData))
            {
                Sprite sp = Sprite.Create(tex, new Rect(0,0, tex.width, tex.height), Vector2.zero);
                homework.sprite = sp;
                GetComponent<EasyDraw>().CanvasInit();
            }
            else
            {
                Debug.LogError("could not decode image");
            }
        }
        else
        {
            Debug.LogError("Error getting image:" + dataUrl);
        }
    }
}
