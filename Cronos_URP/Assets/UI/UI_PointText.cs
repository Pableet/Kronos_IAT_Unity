using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_PointText : UI_TPCP
{
    //[SerializeField]
    public TextMeshProUGUI textTP;
    public TextMeshProUGUI textCP;


    private void Start()
    {
        Bind<TextMeshProUGUI>(typeof(Texts));
    }

    int tp = 99;

    public void OnButtonClicked()
    {
        Debug.Log("Button Clicked");

        tp--;
        textTP.text = tp.ToString("0000");
    }
}
