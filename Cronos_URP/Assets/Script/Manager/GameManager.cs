using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("GameManager");
                    _instance = go.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SwitchScene(string NextScene)
    {
        SceneManager.LoadScene(NextScene);
        GameObject temp = GameObject.Find("Player");
        temp.SetActive(false);
        temp.SetActive(true);
        //temp.GetComponent<Player>().StartPlayer();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SetCursorInactive();
        }
    }
    void SetCursorInactive()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; // 마우스 안보이게 하기
    }
    
}
