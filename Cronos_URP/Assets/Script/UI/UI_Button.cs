using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;

public class UI_Button : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI text;

    enum Buttons
    {
        Button
    }

    enum Texts
    {
        Text,
        TP
    }

    private void Start()
    {
        Bind(typeof(Buttons));
    }

    void Bind(Type type)
    {

    }

    int tp = 99;

    public void OnButtonClicked()
    {
        Debug.Log("Button Clicked");

        tp--;
        text.text = tp.ToString("0000");
    }
}
