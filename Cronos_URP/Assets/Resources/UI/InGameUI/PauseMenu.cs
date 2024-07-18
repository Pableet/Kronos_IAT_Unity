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
    [SerializeField]
    GameObject titlePanel;
    bool isPaused;
    bool isOption;
    bool isControl;
    bool isLoad;
    bool isTitle;

    private void OnEnable()
    {
        pauseManager = PauseManager.Instance;
        pausePanel.SetActive(false);
        optionPanel.SetActive(false);
        loadPanel.SetActive(false);
        titlePanel.SetActive(false);
        isPaused = false;
    }

    public void OpenOption()
    {
        optionPanel.SetActive(true);
        isOption = true;
    }

    public void OpenControl()
    {
        controlPanel.SetActive(true);
        isControl = true;
    }

    public void OpenCheckPoint()
    {
        loadPanel.SetActive(true);
        isLoad = true;
    }

    public void ExitTitle()
    {
        titlePanel.SetActive(true);
        isTitle = true;
    }

    public void Close()
    {
        pausePanel.SetActive(false);
        isPaused = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                pausePanel.SetActive(true);
                isPaused = true;
                pauseManager.PauseGame();
                Debug.Log("퍼즈메뉴열기");
            }
            else if (isOption)
            {
                optionPanel.SetActive(false);
                isOption = false;
            }
            else if (isControl)
            {
                controlPanel.SetActive(false);
                isControl = false;
            }
            else if (isLoad)
            {
                loadPanel.GetComponent<LoadPanel>().ExitLoad();
                isLoad = false;
            }
            else if (isTitle)
            {
                titlePanel.GetComponent<LoadPanel>().ExitLoad();
                isTitle = false;
            }
            else
            {
                pausePanel.SetActive(false);
                optionPanel.SetActive(false);
                isPaused = false;
                pauseManager.UnPauseGame();
                Debug.Log("퍼즈메뉴닫기");
            }
        }
    }
}
