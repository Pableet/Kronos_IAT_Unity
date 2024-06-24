using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    public static PauseManager pauseManager;
    public static PlayerInput playerInput;

    public static bool isPaused { get; private set; }

    private void Awake()
    {
        if (pauseManager == null)
            pauseManager = this;

        playerInput = GetComponent<PlayerInput>();
    }

    public static void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        playerInput.SwitchCurrentActionMap("UI");
    }

    public static void UnPauseGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        playerInput.SwitchCurrentActionMap("Player");
    }
}
