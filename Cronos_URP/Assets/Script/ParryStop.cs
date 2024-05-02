using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ParryStop : MonoBehaviour
{
    float speed;
    bool bRestoreTime;

    public GameObject ParryEffect;
    GameObject ParryInst;
    Animator animator;
    public GameObject Player;


    private void Start()
    {
        bRestoreTime = false;
        animator = GetComponentInChildren<Animator>();
        ParryInst = Instantiate(ParryEffect);
        ParryInst.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (bRestoreTime)
        {
            if (Time.timeScale < 1.0f)
            {
                Time.timeScale += Time.deltaTime * speed;
            }
            else
            {
                Time.timeScale = 1.0f;
                bRestoreTime = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.Play("Parry");
        }
    }

    public void StopTime(float changeTime, int restoreSpeed, float delay)
    {
        speed = restoreSpeed;

        if (delay > 0) 
        {
            StopCoroutine(StartTimeAgain(delay));
            StartCoroutine(StartTimeAgain(delay));
        }
        else
        {
            bRestoreTime = true;
        }

        Debug.Log("Stop Time");

        ParryInst.transform.position = Player.transform.position;
        ParryInst.SetActive(true);
        Time.timeScale = changeTime;
    }

    IEnumerator StartTimeAgain(float amount)
    {
        yield return new WaitForSecondsRealtime(amount);
        bRestoreTime = true;
        ParryInst.SetActive(false);
    }

    void ParryTest()
    {
        StopTime(0.05f, 10, 1f);
    }
}
