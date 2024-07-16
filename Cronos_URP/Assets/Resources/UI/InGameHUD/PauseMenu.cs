using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    PauseManager pauseManager;

    [SerializeField]
    GameObject pausePanel;
    [SerializeField]
    GameObject optionPanel;
    [SerializeField]
    GameObject controlPanel;
    [SerializeField]
    GameObject loadPanel;
    bool isPanelOpened;

    private void OnEnable()
    {
        pauseManager = PauseManager.Instance;
        pausePanel.SetActive(false);
        isPanelOpened = false;
    }

    public void OpenOption()
    {
        optionPanel.SetActive(true);
    }

    public void OpenControl()
    {

    }

    public void LoadCheckPoint()
    {

    }

    public void ExitTitle()
    {

    }

    public void Close()
    {
        pausePanel.SetActive(false);
        isPanelOpened = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPanelOpened)
            {
                pausePanel.SetActive(true);
                isPanelOpened = true;
                pauseManager.PauseGame();
                Debug.Log("퍼즈메뉴열기");
            }
            else
            {
                pausePanel.SetActive(false);
                optionPanel.SetActive(false);
                isPanelOpened = false;
                pauseManager.UnPauseGame();
                Debug.Log("퍼즈메뉴닫기");
            }
        }
    }
}
