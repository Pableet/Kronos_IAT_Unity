using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Button : UI
{
    [SerializeField]
    TextMeshProUGUI text;


    private void Start()
    {
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));

        GetText((int)Texts.ButtonText).text = "Bind Test";
    }

    int tp = 99;

    public void OnButtonClicked()
    {
        Debug.Log("Button Clicked");

        tp--;
        text.text = tp.ToString("0000");
    }
}
