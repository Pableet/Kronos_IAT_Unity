using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;


public class UI_Scanner : MonoBehaviour
{
    GameObject player;
    GameObject interText;
    bool isPopup;

    private void Awake()
    {
        player = GameObject.Find("Player");
        interText = GameObject.Find("UI_Interact");
        isPopup = false;
    }

    private void Start()
    {
        interText.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            Debug.Log("Trigger");
            interText.SetActive(true);
            isPopup = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            Debug.Log("Trigger Exit");
            interText.SetActive(false);
            isPopup = false;
        }
    }

    void Update()
    {
        if (isPopup && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Player Interact");
            UI_PowerUp.PowerUp();
            //Destroy(gameObject);
            interText.SetActive(false);
        }
    }
}
