using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;


public class UI_Scanner : MonoBehaviour
{
    GameObject player;
    GameObject interText;

    private void Start()
    {
        player = GameObject.Find("Player");
        interText = GameObject.Find("UI_Interact");
        interText.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            Debug.Log("Trigger");
            interText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            Debug.Log("Trigger Exit");
            interText.SetActive(false);
        }
    }
}
