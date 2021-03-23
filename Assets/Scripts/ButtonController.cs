using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public int X = 500;
    public int Y = 100;
    public int TextSize = 72;
    public float scale = 1.5f;

    public Button button;
    public void ExtendButton(Button button)
    {
        button.image.rectTransform.sizeDelta = new Vector3(X * scale, Y * scale);
        //button.Text.fontSize = TextSize * scale;
    }
    public void DefaultButton(Button button)
    {
        button.image.rectTransform.sizeDelta = new Vector3(X, Y);
        //button.Text.fontSize = TextSize;
    }

}