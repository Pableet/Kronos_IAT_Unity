using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("MICScene");

        // 빌드 인덱스로 씬을 전환하는 구현
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitGame()
    {
        Debug.Log("께임 종료!");
        Application.Quit();
    }
}
