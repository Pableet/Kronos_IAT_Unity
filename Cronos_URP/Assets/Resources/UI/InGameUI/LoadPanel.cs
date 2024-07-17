using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadPanel : MonoBehaviour
{
    [SerializeField]
    Button option;
    [SerializeField]
    Button control;
    [SerializeField]
    Button load;
    [SerializeField]
    Button title;

    private void OnEnable()
    {
        option.gameObject.SetActive(false);
        control.gameObject.SetActive(false);
        load.gameObject.SetActive(false);
        title.gameObject.SetActive(false);
    }

    public void LoadSave()
    {

    }

    public void GoTitle()
    {

    }

    public void ExitLoad()
    {
        gameObject.SetActive(false);
        option.gameObject.SetActive(true);
        control.gameObject.SetActive(true);
        load.gameObject.SetActive(true);
        title.gameObject.SetActive(true);
    }
}
