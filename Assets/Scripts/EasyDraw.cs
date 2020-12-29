/*
 Copyright 2020, Ivan Ovchinnikov.
 All rights reserved.
 
 All the code below or any of it's parts belong to it's author (Ivan Ovchinnikov)
 And URIMP llc, any use without direct permission of it's author is restricted.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EasyDraw : MonoBehaviour
{
    private Image pic;
    private RectTransform selectedCanvas;
    private Vector2 lastPoint;
    private bool inDrawMode;
    public Texture2D brush;
    public Sprite[] modeKeys;
    public Image drawKey;
    private Color[] pix;
    private Color red = Color.red;
    private Texture2D canvas;

    private void Start()
    {
        pic = GetComponent<Image>();
        selectedCanvas = GetComponent<RectTransform>();
        pix = brush.GetPixels();
        //CanvasInit();
    }

    public void CanvasInit()
    {
        canvas = Instantiate((Texture2D)pic.sprite.texture);
    }

    public void DrawMode()
    {
        inDrawMode = !inDrawMode;
        if (inDrawMode) drawKey.sprite = modeKeys[1];
        else drawKey.sprite = modeKeys[0];
    }

    public void DrawInit()
    {
        if (!inDrawMode) return;
        //if (pic.sprite == null)
        //{
        //    pic.sprite = null;
        //    if (canvas) Destroy(canvas);
        //    GetComponent<LoadHomework>().DrawJSLayout();
        //}
        //else
        //{
            Vector2 pointer;
            Vector2 position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(selectedCanvas, position, null, out pointer))
                lastPoint = new Vector2((int)pointer.x, (int)pointer.y);
        //}
    }

    public void Draw()
    {
        if (pic.sprite == null || !inDrawMode) return;
        Vector2 pointer;
        Vector2 position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(selectedCanvas, position, null, out pointer) &&
            (pointer.x >= 0 && pointer.x <= selectedCanvas.rect.width) &&
            (pointer.y >= 0 && pointer.y <= selectedCanvas.rect.height))
        {
            Debug.Log(pointer);
            int x = (int)pointer.x;
            int y = (int)pointer.y;
            Vector2 point = lastPoint;
            float step = 1 / Mathf.Sqrt(Mathf.Pow(x - lastPoint.x, 2) + Mathf.Pow(y - lastPoint.y, 2));
            float currentStep = 0;
            while ((int)point.x != x || (int)point.y != y)
            {
                point = Vector2.Lerp(lastPoint, new Vector2(x, y), currentStep);
                currentStep += step;
                int index1 = 0;
                for (int index2 = -brush.height / 2; index2 < brush.height / 2; ++index2)
                {
                    for (int index3 = -brush.width / 2; index3 < brush.width / 2; ++index3)
                    {
                        if ((double)pix[index1].a > 0.1)
                        {
                            Color pixel = canvas.GetPixel((int)point.x + index3, (int)point.y + index2);
                            canvas.SetPixel((int)point.x + index3, (int)point.y + index2, Color.Lerp(pixel, red, pix[index1].a));
                        }
                        ++index1;
                    }
                }
            }
            canvas.Apply();
            pic.sprite = Sprite.Create(canvas, new Rect(0, 0, canvas.width, canvas.height), Vector2.zero);
            lastPoint = new Vector2(x, y);
        }
    }

}
